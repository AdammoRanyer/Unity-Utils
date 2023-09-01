using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
	public Player p1;
	
    // Start is called before the first frame update
    void Start()
    {
        p1 = new Player();
    }

    // Update is called once per frame
    void Update()
    {
        p1.move();
		p1.animationsControl();
		p1.colisions();
		p1.update();
    }
	
	public class Player{
		public GameObject utils;
		public GameObject obj;
		public float speed;
		public float hspeed;
		public float vspeed;
		
		public Player(){
			this.utils = GameObject.Find("Main Camera");
			this.obj = GameObject.Find("p1");
			this.hspeed = 0;
			this.vspeed = 0;
			this.speed = 16;
		}
		
		public void move(){
			if(this.utils.GetComponent<main>().GC.paused == false){
				if(this.utils.GetComponent<utils>().inputKey(this.utils.GetComponent<main>().GC.key_downArrow, 0)){
					this.vspeed -= this.speed;
				}
				if(this.utils.GetComponent<utils>().inputKey(this.utils.GetComponent<main>().GC.key_upArrow, 0)){
					this.vspeed += this.speed;
				}
				if(this.utils.GetComponent<utils>().inputKey(this.utils.GetComponent<main>().GC.key_rightArrow, 0)){
					this.hspeed += this.speed;
				}
				if(this.utils.GetComponent<utils>().inputKey(this.utils.GetComponent<main>().GC.key_leftArrow, 0)){
					this.hspeed -= this.speed;
				}
			}
		}
		
		public void update(){
			this.utils.GetComponent<utils>().set_position(
				this.obj, 
				new Vector3(obj.transform.position.x+this.hspeed, obj.transform.position.y+this.vspeed, obj.transform.position.z)
			);
			this.hspeed = 0;
			this.vspeed = 0;
		}
		
		public void animationsControl(){
			if(this.utils.GetComponent<main>().GC.paused == false){
				if(this.obj.GetComponent<Animator>().enabled == false){
					this.obj.GetComponent<Animator>().enabled = true;
				}
			}else{
				if(this.obj.GetComponent<Animator>().enabled == true){
					this.obj.GetComponent<Animator>().enabled = false;
				}
			}
		}
		
		public void colisions(){
			Vector2 hvspeed = this.utils.GetComponent<utils>().boxColision(this.obj, new Vector2(this.hspeed, this.vspeed), "Tilemap_colisions");
			this.hspeed = hvspeed[0];
			this.vspeed = hvspeed[1];
		}
	}
}
