#pragma strict
@script RequireComponent(AudioSource);
var _idleSounds:AudioClip[];
var _idleSoundRandomChance:float = .05;

var _flightSounds:AudioClip[];
var _flightSoundRandomChance:float = .05;


var _scareSounds:AudioClip[];
var _pitchMin:float = .85;
var _pitchMax:float = 1;

var _volumeMin:float = .6;
var _volumeMax:float = .8;

private var _flockChild:FlockChild;
private var _audio:AudioSource;
private var _hasLanded:boolean;

function Start () {
	_flockChild = GetComponent(FlockChild);
	_audio = GetComponent(AudioSource);
	InvokeRepeating("PlayRandomSound", Random.value+1, 1);	
	if(_scareSounds.Length > 0)
	InvokeRepeating("ScareSound", 1, .01);
}

function PlayRandomSound () {
	if(gameObject.activeInHierarchy){
		if(!_audio.isPlaying && _flightSounds.Length > 0 && _flightSoundRandomChance > Random.value && !_flockChild._landing){
			_audio.clip = _flightSounds[Random.Range(0,_flightSounds.Length)];
			_audio.pitch = Random.Range(_pitchMin, _pitchMax);
			_audio.volume = Random.Range(_volumeMin, _volumeMax);
			_audio.Play();
		}else if(!_audio.isPlaying && _idleSounds.Length > 0 && _idleSoundRandomChance > Random.value && _flockChild._landing){
			_audio.clip = _idleSounds[Random.Range(0,_idleSounds.Length)];
			_audio.pitch = Random.Range(_pitchMin, _pitchMax);
			_audio.volume = Random.Range(_volumeMin, _volumeMax);
			_audio.Play();
			_hasLanded = true;
		}
	}
}

function ScareSound () {	
if(gameObject.activeInHierarchy){
	if(_hasLanded && !_flockChild._landing && _idleSoundRandomChance*2 > Random.value){
		_audio.clip = _scareSounds[Random.Range(0,_scareSounds.Length)];
		_audio.volume = Random.Range(_volumeMin, _volumeMax);
		_audio.PlayDelayed(Random.value*.2);
		_hasLanded = false;
	}
	}
}