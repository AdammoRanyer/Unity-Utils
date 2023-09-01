using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
	public GameConfig GC;
	
    // Start is called before the first frame update
    void Start()
    {
		GC = new GameConfig(
			60,   // FPS
			6,    // resolution
			false // fullscreen
		);
    }

    // Update is called once per frame
    void Update()
    {
        GC.pause();
    }
	
	public class GameConfig{
		public GameObject utils;
		public int FPS;
		public int[] window_originalResolution;
		public int resolution;
		public bool fullscreen;
		public Vector4 backgroundColor;
		public bool paused;
		public string key_return;
		public string key_rightArrow;
		public string key_leftArrow;
		public string key_upArrow;
		public string key_downArrow;
		
		public GameConfig(int FPS, int resolution, bool fullscreen){
			this.utils = GameObject.Find("Main Camera");
			this.FPS = FPS;
			this.window_originalResolution = new int[]{64, 112};
			this.resolution = resolution;
			this.fullscreen = fullscreen;
			this.backgroundColor = new Vector4(0, 0, 0, 0);
			this.paused = false;
			this.key_return = "Return";
			this.key_rightArrow = "RightArrow";
			this.key_leftArrow = "LeftArrow";
			this.key_upArrow = "UpArrow";
			this.key_downArrow = "DownArrow";
			
			set_FPS(this.FPS);
			set_resolution(this.window_originalResolution, this.resolution, this.fullscreen);
			//set_backgroundColor(this.backgroundColor);
		}
		
		public void set_FPS(int new_FPS){
			this.utils.GetComponent<utils>().set_FPS(new_FPS);
			this.FPS = new_FPS;
		}
		
		public void set_resolution(int[] window_originalResolution, int new_resolution, bool new_fullscreen){
			this.utils.GetComponent<utils>().set_resolution(window_originalResolution, new_resolution, new_fullscreen);
			this.resolution = new_resolution;
			this.fullscreen = new_fullscreen;
		}
		
		public void set_backgroundColor(Vector4 new_backgroundColor){
			GameObject camera = GameObject.Find("Main Camera");
			this.utils.GetComponent<utils>().set_backgroundColor(camera, new_backgroundColor);
			this.backgroundColor = new_backgroundColor;
		}
		
		public void pause(){
			if(this.utils.GetComponent<utils>().inputKey(this.key_return, 1) && this.paused == false){
				this.paused = true;
			}else if(this.utils.GetComponent<utils>().inputKey(this.key_return, 1) && this.paused == true){
				this.paused = false;
			}
		}
	}
}
