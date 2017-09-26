using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PC2Parser : MonoBehaviour 
{
	public TextAsset[] rawDataItems;
	public int framerate = 24;
	public int frameOffset = 0;
	public int startingAnimationIndex = 0;
	public Vector3 rotationAdjustment;
	
	public struct PC2Animation
	{
		public int vertexCount;
		public int sampleCount;
		public int count;
		
		public List<Vector3> vertices;
	}
	
	protected PC2Animation[] pc2Animations;
	protected static Dictionary<string, PC2Animation> storedPC2Animations;
	protected bool ready = false;
	protected Mesh mesh;
	
	protected Vector3[] vertices;
	protected float interval;
	protected bool isVisible = true;
	protected float currentFrame = 0;

	// blending
	
	protected int currentIndex = 0;
	protected int targetIndex = 0;
	protected float blendStartTime;
	protected float blendTime = 2.0f;
	
	void Start () 
	{
		if (storedPC2Animations == null)
		{
			storedPC2Animations = new Dictionary<string, PC2Animation>();
		}
		
		transform.Rotate(rotationAdjustment);
		
		interval = 1.0f / framerate;
		mesh = this.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		currentFrame = frameOffset;
		
		currentIndex = targetIndex = startingAnimationIndex;
		
		pc2Animations = new PC2Animation[rawDataItems.Length];
		
		int i = 0;
		
		foreach(TextAsset rawData in rawDataItems)
		{
			string signature = Signature(rawData.bytes, 128);
			
			if (!storedPC2Animations.ContainsKey(signature))
			{
				PC2Animation parsed = ParsePC2(rawData);
				storedPC2Animations.Add(signature, parsed);
			}
			
			PC2Animation stored = storedPC2Animations[signature];
			
			pc2Animations[i++] = stored;
		}
		
		ready = true;
	}
	
	protected string Signature(byte[] bytes, int length)
	{
		byte[] signature = new byte[length];
		System.Array.Copy(bytes, signature, length);
		
		string key = System.Text.Encoding.ASCII.GetString(signature);
		
		return key;
	}
	
	void Update()
	{
		if (!ready || !isVisible)
			return;

		currentFrame += Time.deltaTime * framerate;
		float flooredFrame = Mathf.Floor(currentFrame);
		float t = currentFrame - flooredFrame;
		int intFrame = (int) flooredFrame;
		int count = intFrame * vertices.Length;
		
		bool processed = false;
		
		if (currentIndex != targetIndex)
		{
			float t_blend = (Time.time - blendStartTime) / blendTime;
			
			if (t_blend < 1)
			{
				processed = true;
				
				for (int i = 0; i < vertices.Length; ++i)
				{
					Vector3 vertex1 = pc2Animations[currentIndex].vertices[(count + i) % pc2Animations[currentIndex].count];
					Vector3 vertex2 = pc2Animations[currentIndex].vertices[(count + i + vertices.Length) % pc2Animations[currentIndex].count];
					Vector3 vertex3 = Vector3.Lerp(vertex1, vertex2, t);

					Vector3 vertex4 = pc2Animations[targetIndex].vertices[(count + i) % pc2Animations[targetIndex].count];
					Vector3 vertex5 = pc2Animations[targetIndex].vertices[(count + i + vertices.Length) % pc2Animations[targetIndex].count];
					Vector3 vertex6 = Vector3.Lerp(vertex4, vertex5, t);

					vertices[i] = Vector3.Lerp(vertex3, vertex6, t_blend);
				}
			}
			else
			{
				currentIndex = targetIndex;
			}
		}
		
		if (!processed)
		{
			for (int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertex1 = pc2Animations[currentIndex].vertices[(count + i) % pc2Animations[currentIndex].count];
				Vector3 vertex2 = pc2Animations[currentIndex].vertices[(count + i + vertices.Length) % pc2Animations[currentIndex].count];
				
				vertices[i] = Vector3.Lerp(vertex1, vertex2, t);
			}
		}
		
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
	
	public bool Blend(int index, float duration)
	{
		if (targetIndex != currentIndex)
		{
			return false; // already blending
		}
		
		blendTime = duration;
		blendStartTime = Time.time;
		targetIndex = index;
		
		return true;
	}
	
	protected PC2Animation ParsePC2(TextAsset data)
	{
		PC2Animation anim = new PC2Animation();
		
		MemoryStream stream = new MemoryStream(data.bytes);
		BinaryReader reader = new BinaryReader(stream);
		
		// Skip header
		
		reader.ReadBytes(16);
		
		// Vertex count
		
		anim.vertexCount = reader.ReadInt32();
		
		// Skip irrelevant
		
		reader.ReadBytes(8);
		
		// Sample count
		
		anim.sampleCount = reader.ReadInt32();
		
		// Vertex data
		
		anim.vertices = new List<Vector3>();
		
		for (int i = 0; i < anim.sampleCount; ++i)
		{
			for (int j = 0; j < anim.vertexCount; ++j)
			{
				Vector3 vector = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
				
				anim.vertices.Add(vector);
			}
		}
		
		anim.count = vertices.Length * anim.sampleCount;
		
		stream.Close();
		
		return anim;
	}
	
	void OnBecameVisible()
	{
		isVisible = true;
	}
	
	void OnBecameInvisible()
	{
		isVisible = false;
	}
}