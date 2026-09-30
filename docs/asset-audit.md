# Mill Valley — Asset Audit

*Generated headlessly by Helm — September 30, 2026. Report only; nothing was deleted.*

**Assets/ total:** 5,255 MB in 6,404 files (excluding .meta)

## Key findings (most actionable first)

1. **`Assets/ThirdParty duplicates/Desktop_Ground_Cover_Package` (213 MB) is a strict subset of `Assets/ThirdParty/Desktop_Ground_Cover_Package` (265 MB).** Same four plant folders (Azalea, Backyard_Grass, Cattail, Snake_Weed); the ThirdParty copy also has Elephant_Grass + Western_Sword_Fern. If the scenes reference the ThirdParty GUIDs, the duplicate folder is 4 % of the project for nothing. The `MegaWire` duplicate is just one 44 KB greygrid texture + material (no twin, harmless).
2. **Source textures ship at 4K-8K on a mobile GPU: 56 files > 2048 px, 575 MB.** Worst offenders are three 8192x8192 PNGs in `Materials and Textures/Real Materials` (wood panel/log walls, ~90 MB) and the four 4096x4096 Realistic_Waterfall TIF sheets (256 MB). Setting an Android platform override of Max Size 2048 (ASTC 6x6) in the importers costs nothing visually on Quest and shrinks the APK/OBB and load times considerably.
3. **`Models/Video FX/` contains the same Chaplin clip twice (85.9 MB .mp4 + 30.6 MB .ogv).** Only one format is used at runtime; the other is dead weight in the repo (and in the build if referenced).
4. **`MainScene/LightingData.asset` is 82 MB** - the single biggest non-media file. Fine for desktop, but on Quest it is loaded wholesale. Re-baking with lower lightmap resolution / compressed lightmaps would trim it; keep in mind any relight invalidates it.
5. **Third-party demo/sample content is still in the project:** 30+ `.unity` demo scenes (Bird Flocks, PathMagic, Water FX Pack, Cinemachine Examples, Chairs Pack DemoScene, Realistic_Waterfall/Waterfall) plus `Standard Assets` (123 MB) and the raw `.skp`/`.3ds`/`.dae` source models (~360 MB: `shay.3ds` 53 MB, two SketchUp placeholders 47 MB). Unity only builds referenced assets, so these don't bloat the APK, but they slow every import/reimport and every `git clone`. Candidates for a separate `source-assets` repo or Git LFS.

Only the four scenes listed under EditorBuildSettings (`LoadingScene`, `MainScene`, `OldMillScene`, `tamHigh`) are enabled; everything else under `Assets/**/*.unity` is third-party sample content.

## Top-level folder sizes

| Folder | MB | % |
|---|---:|---:|
| `ThirdParty` | 1,727.2 | 32.9% |
| `Models` | 904.9 | 17.2% |
| `Materials and Textures` | 810.6 | 15.4% |
| `Animations` | 646.4 | 12.3% |
| `ThirdParty duplicates` | 213.4 | 4.1% |
| `Scenes` | 201.4 | 3.8% |
| `Building History in 3D - Semester 03 (Fall 2018)` | 128.9 | 2.5% |
| `Buildings` | 128.5 | 2.4% |
| `Standard Assets` | 123.2 | 2.3% |
| `SoundFX` | 71.5 | 1.4% |
| `Building History in 3D - Semester 01 (Fall 2017)` | 62.2 | 1.2% |
| `Terrain` | 46.6 | 0.9% |
| `Building History in 3D - Semester 02 (Spring 2018)` | 40.7 | 0.8% |
| `PostProcessing` | 31.2 | 0.6% |
| `Other` | 25.7 | 0.5% |
| `Images and Photos` | 21.3 | 0.4% |
| `Plugins` | 20.3 | 0.4% |
| `Sprites (images appear while loading TimeWalk)` | 18.7 | 0.4% |
| `MegaWire` | 15.0 | 0.3% |
| `Materials - TimeWalk` | 5.6 | 0.1% |
| `Prefabs` | 4.3 | 0.1% |
| `Materials` | 3.7 | 0.1% |
| `Cinemachine` | 3.3 | 0.1% |
| `Scripts` | 0.1 | 0.0% |
| `Gizmos` | 0.0 | 0.0% |

## Size by extension (top 15)

| Ext | MB |
|---|---:|
| `.tga` | 1,800.1 |
| `.png` | 881.5 |
| `.tif` | 740.6 |
| `.fbx` | 480.8 |
| `.jpg` | 234.9 |
| `.skp` | 222.6 |
| `.psd` | 117.8 |
| `.asset` | 108.1 |
| `.mp4` | 85.9 |
| `.dae` | 83.1 |
| `.spm` | 80.0 |
| `.mp3` | 69.5 |
| `.bmp` | 57.3 |
| `.exr` | 54.5 |
| `.3ds` | 53.5 |

## Top 30 largest files

| MB | File |
|---:|---|
| 85.9 | `Assets/Models/Video FX/Charles Chaplin - The Bank (1915) excerpt.mp4` |
| 82.3 | `Assets/Scenes/MainScene/LightingData.asset` |
| 64.0 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_CircleSwell_TexturesSheet.tif` |
| 64.0 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_Mist_TextureSheet.tif` |
| 64.0 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_Drop_Texturesheet.tif` |
| 64.0 | `Assets/ThirdParty/House24/House24.tga` |
| 53.5 | `Assets/Models/shay.3ds` |
| 37.5 | `Assets/Materials and Textures/Real Materials/Textures/wood panel lacquered normal.png` |
| 32.7 | `Assets/Materials and Textures/Real Materials/Textures/wood log wall diffuse.png` |
| 32.3 | `Assets/Materials and Textures/Real Materials/Textures/wood log wall normal.png` |
| 30.6 | `Assets/Models/Video FX/Charles Chaplin - The Bank (1915) excerpt.oggtheora.ogv` |
| 30.3 | `Assets/Models/Miwok v01/Hut_Test_Unity.fbx` |
| 27.1 | `Assets/ThirdParty/Train Track Creator Pro/Models/Materials/props_n.tga` |
| 26.7 | `Assets/ThirdParty/Cut_Logs_Pack_01/CL_Source/cut_logs_set_01.fbx` |
| 24.9 | `Assets/Building History in 3D - Semester 01 (Fall 2017)/carnegie-library-1920.skp` |
| 22.9 | `Assets/Buildings/SketchUp Placeholders/Mill Valley 1920 - roads and buildings - SketchUp2015.skp` |
| 22.4 | `Assets/Building History in 3D - Semester 03 (Fall 2018)/hikers-retreat/Materials/planks_mesquite_Diffuse.png` |
| 21.0 | `Assets/ThirdParty/Urban Decal+ Pack/Textures/ManHoleCover_NOR.tif` |
| 20.5 | `Assets/Animations/liam@Idle.fbx` |
| 20.0 | `Assets/Animations/liam@Using A Filing Cabinet.fbx` |
| 17.4 | `Assets/Materials and Textures/Real Materials/Textures/wood panel painted diffuse.png` |
| 16.7 | `Assets/Scenes/MainScene.unity` |
| 16.5 | `Assets/Models/California-Bolinas/California-Bolinas.dae` |
| 16.0 | `Assets/Models/Chairs Pack 1/Textures/WoodChairType1_ColorSpecular.tif` |
| 16.0 | `Assets/Models/Chairs Pack 1/Textures/WoodChairType2_ColorSpecular.tif` |
| 16.0 | `Assets/Models/Chairs Pack 1/Textures/WoodChairType3_ColorSpecular.tif` |
| 16.0 | `Assets/Models/Garden Bench/Textures/garden_bench_grids_color_alpha.tif` |
| 16.0 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_CircleSwell_TexturesSheet_Lowres.tif` |
| 16.0 | `Assets/Models/Garden Bench/Textures/garden_bench_color_specular.tif` |
| 16.0 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_Splashes_TextureSheet.tif` |

## `Assets/ThirdParty duplicates`

2 items, 213.5 MB total.

| Item | MB | Type | Same-named twin in `Assets/ThirdParty`? |
|---|---:|---|---|
| `Desktop_Ground_Cover_Package` | 213.5 | folder | YES |
| `MegaWire` | 0.0 | folder | YES |

## Oversized textures (> 2048 px on either axis)

Scanned 2,342 PNG/JPG/TGA/PSD/TIF source images by header (2 unreadable). **56 textures exceed 2048 px, 575.2 MB on disk.**

Top 25 by size:

| MB | Dims | File |
|---:|---|---|
| 64.0 | 4096x4096 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_CircleSwell_TexturesSheet.tif` |
| 64.0 | 4096x4096 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_Mist_TextureSheet.tif` |
| 64.0 | 4096x4096 | `Assets/ThirdParty/Realistic_Waterfall/Textures/Water_Drop_Texturesheet.tif` |
| 64.0 | 4096x4096 | `Assets/ThirdParty/House24/House24.tga` |
| 37.5 | 8192x8192 | `Assets/Materials and Textures/Real Materials/Textures/wood panel lacquered normal.png` |
| 32.7 | 4096x4096 | `Assets/Materials and Textures/Real Materials/Textures/wood log wall diffuse.png` |
| 32.3 | 4096x4096 | `Assets/Materials and Textures/Real Materials/Textures/wood log wall normal.png` |
| 27.1 | 4096x4096 | `Assets/ThirdParty/Train Track Creator Pro/Models/Materials/props_n.tga` |
| 17.4 | 8192x8192 | `Assets/Materials and Textures/Real Materials/Textures/wood panel painted diffuse.png` |
| 12.4 | 1872x2357 | `Assets/Images and Photos/MV Record - July 31 1920.psd` |
| 12.0 | 4000x2337 | `Assets/Sprites (images appear while loading TimeWalk)/1920DepotSprite.png` |
| 12.0 | 4000x2337 | `Assets/Models/Textures/1920 Depot.png` |
| 11.5 | 3840x2160 | `Assets/PostProcessing/Textures/Lens Dirt/LensDirt02.png` |
| 9.8 | 4800x2987 | `Assets/Materials and Textures/textures MORE/Mill Valley - Depot centered max size - Google Earth Pro image.jpg` |
| 9.6 | 2065x904 | `Assets/Scenes/Tam High Scene/wood-hall-1914.psd` |
| 8.1 | 3840x2160 | `Assets/PostProcessing/Textures/Lens Dirt/LensDirt01.png` |
| 6.7 | 2592x2592 | `Assets/ThirdParty/AC_ATL (Ground)V1/Ground/Ground (5).jpg` |
| 6.3 | 4096x4096 | `Assets/Animations/People - sitting/Woman_D/textures/diffuseWoman_D.png` |
| 5.0 | 3840x2160 | `Assets/PostProcessing/Textures/Lens Dirt/LensDirt03.png` |
| 4.8 | 3840x2160 | `Assets/PostProcessing/Textures/Lens Dirt/LensDirt00.png` |
| 4.5 | 2592x2592 | `Assets/ThirdParty/AC_ATL (Ground)V1/Grass/Grass (28).jpg` |
| 4.3 | 3072x3072 | `Assets/Buildings/KeyStone Block/Textures/Plaster35_NRM_3K.jpg` |
| 4.3 | 3072x3072 | `Assets/Buildings/KeyStone Block/Textures/Plaster35_NRM_3K 1.jpg` |
| 3.9 | 3000x2000 | `Assets/Materials and Textures/textures MORE/wildtextures-old-wood-original-file.jpg` |
| 3.6 | 8192x8192 | `Assets/Materials and Textures/textures/F_Dress_03.png` |

## Scenes

| MB | Scene |
|---:|---|
| 16.68 | `Assets/Scenes/MainScene.unity` |
| 3.29 | `Assets/MegaWire/Scenes/wiretest.unity` |
| 2.69 | `Assets/Scenes/OldMillScene.unity` |
| 0.88 | `Assets/ThirdParty/Realistic_Waterfall/Waterfall.unity` |
| 0.75 | `Assets/ThirdParty/Bird Flocks/Bird Flock Seagull/Scenes/Seagull Flock - Landing Example.unity` |
| 0.54 | `Assets/ThirdParty/Bird Flocks/Bird Flock Crow/Scenes/Crow Flock - Landing Example.unity` |
| 0.22 | `Assets/Scenes/tamHigh.unity` |
| 0.17 | `Assets/ThirdParty/Bird Flocks/Bird Flock Crow/Scenes/Crow Flock - Example.unity` |
| 0.17 | `Assets/ThirdParty/Bird Flocks/Bird Flock Seagull/Scenes/Seagull Flock - Avoidance Example +Sound.unity` |
| 0.17 | `Assets/ThirdParty/Bird Flocks/Bird Flock Seagull/Scenes/Seagull Flock - Avoidance Example.unity` |
| 0.14 | `Assets/Scenes/LoadingScene.unity` |
| 0.13 | `Assets/Materials and Textures/Portal Particle/Scene.unity` |
| 0.10 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo3.unity` |
| 0.09 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo4.unity` |
| 0.08 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo5.unity` |
| 0.07 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo2.unity` |
| 0.06 | `Assets/ThirdParty/Bird Flocks/Bird Flock Seagull/Scenes/Seagull Flock - Model.unity` |
| 0.04 | `Assets/Cinemachine/Examples/Scenes/Noise/noise.unity` |
| 0.03 | `Assets/PostProcessing/Utilities/CustomMotionTexture/ExampleScene.unity` |
| 0.03 | `Assets/Cinemachine/Examples/Scenes/Transposer/transposer.unity` |
| 0.03 | `Assets/Cinemachine/Examples/Scenes/Free Look/Free Look.unity` |
| 0.03 | `Assets/ThirdParty/Bird Flocks/Bird Flock Crow/Scenes/Crow Flock - Crow Model.unity` |
| 0.03 | `Assets/ThirdParty/Water FX Pack/Demo Scenes/Waterfall.unity` |
| 0.03 | `Assets/ThirdParty/Bird Flocks/Included Assets/Unluck Software Feather Particles/Scenes/Feather SCN.unity` |
| 0.03 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo1.unity` |
| 0.02 | `Assets/Cinemachine/Examples/Scenes/Timeline/Timeline.unity` |
| 0.02 | `Assets/Cinemachine/Examples/Scenes/State machine/state_machine.unity` |
| 0.02 | `Assets/Cinemachine/Examples/Scenes/Scripting/Scripting.unity` |
| 0.02 | `Assets/Models/Chairs Pack 1/Scenes/DemoScene.unity` |
| 0.02 | `Assets/Cinemachine/Examples/Scenes/Composer/composer.unity` |
| 0.02 | `Assets/ThirdParty/Water FX Pack/Demo Scenes/SteamSpray.unity` |
| 0.01 | `Assets/ThirdParty/Water FX Pack/Demo Scenes/Rain.unity` |
| 0.01 | `Assets/ThirdParty/Water FX Pack/Demo Scenes/VolumeSteam.unity` |
| 0.01 | `Assets/ThirdParty/PathMagic/Demo Scenes/Demo6.unity` |
| 0.01 | `Assets/ThirdParty/Bird Flocks/Bird Flock Crow/Scenes/Crow Flock - Free Roam.unity` |
| 0.01 | `Assets/ThirdParty/Bird Flocks/Bird Flock Seagull/Scenes/Seagull Flock - Free Roam.unity` |
| 0.01 | `Assets/ThirdParty/Water FX Pack/Demo Scenes/Snow.unity` |

**EditorBuildSettings (enabled scenes):**

- ✅ `Assets/Scenes/LoadingScene.unity`
- ✅ `Assets/Scenes/MainScene.unity`
- ✅ `Assets/Scenes/OldMillScene.unity`
- ✅ `Assets/Scenes/tamHigh.unity`
