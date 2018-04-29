using UnityEngine;
using System.Collections.Generic;
public static class Sound  {

	static Sound() {
		soundObject=new GameObject("Sound");
		GameObject.DontDestroyOnLoad(soundObject);
		//	Debug.Log (FileIO.GetKeyData ("IsBgSound"));
		if (PlayerPrefs.HasKey("AudioVolume"))
		{
			BgVolume = (float)PlayerPrefs.GetInt("AudioVolume") / 100f;
		}
		else
		{
			BgVolume = 0.5f;
		}
		if (PlayerPrefs.HasKey("SoundVolume"))
		{
			Volume = (float)PlayerPrefs.GetInt("SoundVolume") / 100f;
		}
		else
		{
			Volume = 0.5f;
		}
	}

	static GameObject soundObject;

	private static void SourceController(System.Action<SoundBehaviour> action,string str=null) {
		SoundBehaviour[] audioSources = soundObject.GetComponents<SoundBehaviour> ();
		for (int i = 0; i < audioSources.Length; i++) {
			if(str==null || audioSources[i].Name==str) {
				action(audioSources[i]);
			}
		}
	}

    static float _volume;
    static float _Bgvolume;
    public static float Volume {
		get
        {
            return _volume;
        }

		set
        {
			_volume = value;
			SourceController(audio => { 
				if (audio != bgMusic) {
					audio.Volume=audio.Volume;
				}
			});
			PlayerPrefs.SetInt("SoundVolume", (int)(value * 100));

		}

	}

    public static float BgVolume
    {
        get
        {
            return _Bgvolume;
        }

        set
        {
			_Bgvolume = value;
			SourceController(audio => { if (audio == bgMusic) audio.Volume = audio.Volume; });
			PlayerPrefs.SetInt("AudioVolume", (int)(value * 100));
		}

    }

    static float _pitch=1f;
	public static float Pitch {
		get {
			return _pitch;
		}
		set {
			_pitch = value;
			SourceController(audio=>audio.audioSource.pitch=value);
		}
	}

	static bool _bMute=false;
	public static bool IsMute {
		get {return _bMute;}
		set {
			
			_bMute=value;
			
			SourceController(audio=>audio.audioSource.mute=value);
			
		}
	}

	static bool _bBg=true;
	public static bool bBg {
		get {return _bBg;}
		set {

			_bBg=value;

			if (bgMusic)
				bgMusic.audioSource.mute = !value;
		

		}
	}

	static bool _bEffect=true;
	public static bool bEffect {
		get {return _bEffect;}
		set {

			_bEffect=value;

			SourceController(audio=>{if(audio!=bgMusic)audio.audioSource.mute=!value;});

		}
	}


	static SoundBehaviour bgMusic=null;

    public static string GetBgName()
    {
        if(bgMusic == null) return null;
		return bgMusic.Name;
    }

	public static float GetBgVolume() {
		float vol = 1f;
		if (bgMusic!=null) {
			return bgMusic.Volume;
		} else
			return 1f;


	}

	public static void StopBg() {
        //if(Config.a.IsSoundDebug)
		    Debug.Log ("Stop Bg ");
        if (bgMusic) {
			bgMusic.Stop ();
			bgMusic = null;
		}

	}


	public static void Stop() {
        //if (Config.a.IsSoundDebug)
            Debug.Log ("StopAll ");
		SourceController (t => {
			t.Stop ();
		});
			
		bgMusic = null;

	}

	public static void Stop(string str) {
        //if (Config.a.IsSoundDebug)
            Debug.Log ("StopSound " + str);
		SourceController(t=> {
			t.Stop();

		},str);

	}

    public static void ChangeVolume(float vol, string name=null)
    {
        if(string.IsNullOrEmpty(name))
        {
            if (bgMusic != null)
            {
				bgMusic.Volume = vol;
            }
        } else
        {
            SourceController(t =>
            {
			
				t.Volume = vol;
            },name);
        }
    }

    public static void PauseBg()
	{
		if (bgMusic)
		{
			bgMusic.audioSource.Pause();
		}
	}

	public static void ResumeBg()
	{
		if (bgMusic)
		{
			bgMusic.audioSource.Play();
		}
	}

	static void AsyncPlay( AudioClip clip, bool bLoop, bool bBg=false) {
		SoundBehaviour sb;
		if( bBg ) {
			if (bgMusic==null)
				bgMusic =soundObject.AddComponent<SoundBehaviour>();
			bgMusic.IsBgSound = true;
			sb=bgMusic;
		} else {
			sb=soundObject.AddComponent<SoundBehaviour>();
		}
		sb.SetSound(clip,bLoop,Pitch, Sound.IsMute);
		
	}


	public static void PlayBg(string str, bool bLoop = true) {
        if (string.IsNullOrEmpty(str)) return;
		#if true
		AudioClip clip = null;

		clip=Resources.Load("Sounds/"+str) as AudioClip;
		if (clip == null) {
            //if (Config.a.IsSoundDebug)
                Debug.LogError ("Can't Find SoundClip = " + str);
		
		}

		if (bgMusic==null) {
			bgMusic =soundObject.AddComponent<SoundBehaviour>();
			bgMusic.IsBgSound=true;
		}
		bgMusic.SetSound( 
			clip,
			bLoop,
			Pitch,
			Sound.IsMute);
        bgMusic.Volume = 1f; 

#else 
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
					
					AsyncPlay( ac,bLoop,true);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
							
							AsyncPlay( ac,bLoop,true);
						}
					};

				}
			};
		}
		else {
			
			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac= ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
					
					AsyncPlay(ac,bLoop,true);
				} 
			};
		}

#endif
      
	}

	public static void Play(string str, bool bLoop=false) {
        
		if (string.IsNullOrEmpty(str)) return;
#if true
        AudioClip clip = null;
        
		clip = Resources.Load("Sounds/" + str) as AudioClip;
		if (clip == null) {
            //if (Config.a.IsSoundDebug)
                Debug.LogError ("Can't Find SoundClip = " + str);

		}
        
		soundObject.AddComponent<SoundBehaviour>().SetSound( 
			clip,
			bLoop,
			Pitch,
			Sound.IsMute);

#else
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
				
					AsyncPlay( ac,bLoop);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
						
							AsyncPlay( ac,bLoop);
						}
					};

				}
			};
		}
		else {

			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac=  ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
				
					AsyncPlay( ac,bLoop);
				} 
			};
		}

#endif

	}
#region BundleAudio
	public static void PlayBg(string bundle, string str, bool bLoop = true)
	{
		if (string.IsNullOrEmpty(str)) return;
#if true
		AudioClip clip = null;

		//clip = AssetBundleManager.a.GetAsset<AudioClip>(bundle, "Sounds/" + str + ".mp3") as AudioClip;
		//if (!clip)
		//	clip = AssetBundleManager.a.GetAsset<AudioClip>(bundle, "Sounds/" + str + ".wav") as AudioClip;

		if (!clip)
			clip = Resources.Load("Sounds/" + str) as AudioClip;

		if (clip == null)
		{
            //if (Config.a.IsSoundDebug)
                Debug.LogError("Can't Find SoundClip = " + str);

		}

		if (bgMusic == null)
		{
			bgMusic = soundObject.AddComponent<SoundBehaviour>();
			bgMusic.IsBgSound = true;
		}
		bgMusic.SetSound(
			clip,
			bLoop,
			Pitch,
			Sound.IsMute);
		bgMusic.Volume = 1f;

#else
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
					
					AsyncPlay( ac,bLoop,true);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
							
							AsyncPlay( ac,bLoop,true);
						}
					};

				}
			};
		}
		else {
			
			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac= ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
					
					AsyncPlay(ac,bLoop,true);
				} 
			};
		}

#endif

	}


	public static void Play(string bundle,string str, bool bLoop = false)
	{

		if (string.IsNullOrEmpty(str)) return;
#if true
		AudioClip clip = null;

		
		//clip = AssetBundleManager.a.GetAsset<AudioClip>(bundle,"Sounds/" + str + ".mp3") as AudioClip;
		//if (!clip)
		//	clip = AssetBundleManager.a.GetAsset<AudioClip>(bundle,"Sounds/" + str + ".wav") as AudioClip;
		if (!clip) clip = Resources.Load("Sounds/" + str) as AudioClip;
		if (clip == null)
		{
            //if (Config.a.IsSoundDebug)
                Debug.LogError("Can't Find SoundClip = " + str);

		}

		soundObject.AddComponent<SoundBehaviour>().SetSound(
			clip,
			bLoop,
			Pitch,
			Sound.IsMute);

#else
		if (bAssetBundle)
		{
			AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".mp3").completed+=(a) =>{
				AudioClip ac= ((AssetBundleRequest)a).asset as AudioClip;
				if(ac!=null) {
				
					AsyncPlay( ac,bLoop);
				} else {
					AssetBundleManager.a.GetAssetAsync<AudioClip>("Sounds/" + str + ".wav").completed+=(b) =>{
						ac= ((AssetBundleRequest)b).asset as AudioClip;
						if(ac!=null) {
						
							AsyncPlay( ac,bLoop);
						}
					};

				}
			};
		}
		else {

			Resources.LoadAsync<AudioClip>("Sounds/"+str).completed+=(a) =>{
				AudioClip ac=  ((ResourceRequest)a).asset as AudioClip;
				if(ac!=null) {
				
					AsyncPlay( ac,bLoop);
				} 
			};
		}

#endif

	}
#endregion
#region PrefabAudio
	public static void PlayBg(AudioSource audioSource, bool bLoop = true)
	{
		if (bgMusic == null)
		{
			bgMusic = soundObject.AddComponent<SoundBehaviour>();
			bgMusic.IsBgSound = true;
		}
		bgMusic.SetSound(
			audioSource,
			bLoop,
			Pitch,
			Sound.IsMute);
		bgMusic.Volume = 1f;

	}

	public static void Play(AudioSource audioSource, bool bLoop = false)
	{
		soundObject.AddComponent<SoundBehaviour>().SetSound(
			audioSource,
			bLoop,
			Pitch,
			Sound.IsMute);

	}
#endregion
}

public class SoundBehaviour : MonoBehaviour {
	public bool IsBgSound=false;
	
	public AudioSource audioSource=null;
	bool bDynamicLoadSource = true;

	public string Name {
		get {
			return audioSource.clip.name;
		}

	}

	float volume=1f;
	public float Volume {
		get {
			return volume;
		}

		set {
			volume = value;

			if(IsBgSound)
				audioSource.volume = volume * Sound.BgVolume;
			else 
				audioSource.volume = volume * Sound.Volume;
		}
	}

	public void SetSound(AudioSource audio, bool bLoop=false, float pitch=1f, bool bMute = false)
	{
		if (audioSource != null && bDynamicLoadSource)
		{
			Destroy(audioSource);
		}
		bDynamicLoadSource = false;
		audioSource = audio;
		audioSource.loop = bLoop;
		audioSource.pitch = pitch;
		audioSource.mute = bMute;
		Volume = Volume;
		audioSource.Play();
	}

	public void SetSound(AudioClip clip, bool bLoop=false, float pitch=1f,  bool bMute = false)
	{
		if (audioSource != null && bDynamicLoadSource) {
			Destroy(audioSource);
		}
		if (clip != null) {
			bDynamicLoadSource = true;
			audioSource = gameObject.AddComponent<AudioSource> ();
			audioSource.clip = clip;
			audioSource.loop = bLoop;
			audioSource.pitch = pitch;
			audioSource.mute = bMute;
			Volume = Volume;
			audioSource.Play ();
		} else {
			Stop ();
		}
	}

	public void Stop() {
		
		if(audioSource!=null && bDynamicLoadSource) {
            //if (Config.a.IsSoundDebug)
                Debug.Log ("Stop Sound " + Name);
			Destroy(audioSource);
		}
		Destroy (this);
	}

	void Update() {
		if (audioSource==null || (!IsBgSound && !audioSource.isPlaying)) {
			Stop ();
		}
	}

}



