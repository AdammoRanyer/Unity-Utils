using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileBackground : MonoBehaviour
{
	public TileBackground TB;
	
    // Start is called before the first frame update
    void Start()
    {		
		TB = new TileBackground();
		TB.draw_tilemap(TB.tilemap_1, TB.tilemapCode_1);
		TB.draw_tilemap(TB.tilemap_2, TB.tilemapCode_0);
    }

    // Update is called once per frame
    void Update()
    {
		TB.scroll();
    }
	
	public class TileBackground{
		public GameObject utils;
		public GameObject tilemap_1;
		public GameObject tilemap_2;
		public TileBase[] tiles;
		public int[] tilemapCode_0;
		public int[] tilemapCode_1;
		public float speed;
		
		public TileBackground(){
			this.utils = GameObject.Find("Main Camera");
			this.tilemap_1 = GameObject.Find("Tilemap_1");
			this.tilemap_2 = GameObject.Find("Tilemap_2");
			this.tiles = new TileBase[]{
				Resources.Load<Tile>("Tiles/backgrounds_0"),
				Resources.Load<Tile>("Tiles/backgrounds_1"),
				Resources.Load<Tile>("Tiles/backgrounds_2"),
				Resources.Load<Tile>("Tiles/backgrounds_3"),
				Resources.Load<Tile>("Tiles/backgrounds_4"),
				Resources.Load<Tile>("Tiles/backgrounds_5"),
				Resources.Load<Tile>("Tiles/backgrounds_6"),
				Resources.Load<Tile>("Tiles/backgrounds_7"),
				Resources.Load<Tile>("Tiles/backgrounds_8")
			};
			this.tilemapCode_0 = new int[]{
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0
			};
			this.tilemapCode_1 = new int[]{
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 4, 5, 5, 5, 5, 6, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0,
				0, 1, 2, 2, 2, 2, 3, 0
			};
			this.speed = 1;
		}
		
		public void draw_tilemap(GameObject obj, int[] tilemap){
			int index = 0;
			for(int y = 0; y < 14; y++){
				for(int x = 0; x < 8; x++){
					obj.GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), tiles[tilemap[index]]);
					index += 1;
				}
			}
		}
		
		public void set_position(GameObject obj, Vector3 new_position){
			this.utils.GetComponent<utils>().set_position(obj, new_position);
		}
		
		public void scroll(){
			if(this.utils.GetComponent<main>().GC.paused == false){
				this.set_position(this.tilemap_1, new Vector3(0, (this.tilemap_1.transform.position.y+this.speed), 0));
				if(this.tilemap_1.transform.position.y >= 224){
					this.set_position(this.tilemap_1, new Vector3(0, (this.tilemap_1.transform.position.y-448), 0));
				}

				this.set_position(this.tilemap_2, new Vector3(0, (this.tilemap_2.transform.position.y+this.speed), 0));
				if(this.tilemap_2.transform.position.y >= 224){
					this.set_position(this.tilemap_2, new Vector3(0, (this.tilemap_2.transform.position.y-448), 0));
				}
			}
		}
	}
}
