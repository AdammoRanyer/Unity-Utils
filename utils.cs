using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void set_FPS(int new_FPS){
		/*
		Defini valor para taxa de frames por segundo.
		
		Parâmetros:
			new_FPS (int): taxa de frames por segundo, pré-definida como 60
			
		Retorno:
			FPS definido
		*/
		
		Application.targetFrameRate = new_FPS;
	}
	
	public void set_resolution(int[] window_originalResolution, int new_resolution, bool fullscreen){
		/*
		Atualiza resolução da tela.
		
		Parâmetros:
			new_resolution (int) - escala da nova resolução
			fullscreen (bool) - ativa/desativa tela cheia 
			
		Retorno:
			Resolução atualizada
		*/
		
		int width = window_originalResolution[0] * new_resolution;
		int height = window_originalResolution[1] * new_resolution;
		Screen.SetResolution(width, height, fullscreen);
	}
	
	public void set_backgroundColor(GameObject camera, Vector4 new_colorRGBA){
		/*
		Defini cor de background do objeto camera.
		
		Parâmetros:
			camera (GameObject) - o objeto camera
			new_colorRGBA (Vector4) - a nova cor no formato RGBA 0-255
			
		Retorno:
			Cor de background atualizado
		*/
		
		Color new_color = new Vector4(new_colorRGBA[0] / 1f, new_colorRGBA[1] / 1f, new_colorRGBA[2] / 1f, new_colorRGBA[3] / 1f);
		camera.GetComponent<Camera>().backgroundColor = new_color;
	}
	
	public void set_position(GameObject obj, Vector3 new_position){
		/*
		Atualiza posição de um objeto.
		
		Parâmetros:
			obj (GameObject) - o objeto
			new_position (Vector3) - nova posição
			
		Retorno:
			Posição de objeto atualizada
		*/
		
		obj.transform.position = new_position;
	}
	
	public bool inputKey(string key, int mode){
		/*
		Valida se a tecla de entrada e veradeira ou falsa de acordo com o modo definido.
		
		Parâmetros:
			key (string): a tecla de entrada que será validade como verdeira ou falsa
			mode (int): modo de validação da tecla. 0 para tecla mantida pressionada, 1 para click na tecla
		
		Retorno:
			Tecla de entreda validada
		*/	
		
		bool choice = false;
		if(key.Length > 1){
			KeyCode keyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), key);
			if(mode == 0){
				choice = Input.GetKey(keyCode);
			}else if(mode == 1){
				choice = Input.GetKeyDown(keyCode);
			}		
		}else{
			if(mode == 0){
				choice = Input.GetKey(key);
			}else if(mode == 1){
				choice = Input.GetKeyDown(key);
			}	
		}

		return choice;
	}
	
	public Vector2 boxColision(GameObject obj, Vector2 hvspeed, string collisionObject){
		/*
		
		Parâmetros:
			
			
		Retorno:
			
		*/
		
		Vector2 boxSize = obj.GetComponent<BoxCollider2D>().size;
		
		Vector3 origin = new Vector3((obj.transform.position.x-(boxSize[0]/2)), obj.transform.position.y+(boxSize[1]/2), obj.transform.position.z);
		Vector2 direction = new Vector2(0, -1);
		float distance = boxSize[1];
		RaycastHit2D[] rayCast = Physics2D.RaycastAll(origin, direction, distance);
		Color color = Color.yellow;
		foreach(RaycastHit2D hit in rayCast){
			if(hit.collider.gameObject.name == collisionObject){
				color = Color.red;
				//while(hit.distance > 0){set_position(obj, new Vector3(obj.transform.position.x-1, obj.transform.position.y, obj.transform.position.z));}
				if(hvspeed[0] <= 0){
					hvspeed[0] = 0;
				}
				break;
			}
		}
		Debug.DrawRay(origin, direction * distance, color);
		
		return hvspeed;
	}
}
