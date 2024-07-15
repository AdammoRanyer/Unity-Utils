using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Utils: MonoBehaviour
{
	public void Set_FPS(int new_FPS){
		/*
		Defini valor para taxa de frames por segundo.
		
		Parâmetros:
			new_FPS (int): taxa de frames por segundo, pré-definida como 60
			
		Retorno:
			FPS definido
		*/
		
		Application.targetFrameRate = new_FPS;
	}
	
	public void set_resolution(int[] screen_originalResolution, int new_resolution, bool fullscreen){
		/*
		Atualiza resolução da tela.
		
		Parâmetros:
			screen_originalResolution (int[]) - tamanho original da tela
			new_resolution (int) - escala da nova resolução
			fullscreen (bool) - ativa/desativa tela cheia 
			
		Retorno:
			Resolução atualizada
		*/
		
		int width = screen_originalResolution[0] * new_resolution;
		int height = screen_originalResolution[1] * new_resolution;
		Screen.SetResolution(width, height, fullscreen);
	}
	
	public Vector4 ConvertColor(Vector4 RGBA){
		Vector4 converted_color = new Vector4(RGBA[0] / 255f, RGBA[1] / 255f, RGBA[2] / 255f, RGBA[3] / 255f);
		
		return converted_color;
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
		
		Color new_color = new Vector4(new_colorRGBA[0] / 255f, new_colorRGBA[1] / 255f, new_colorRGBA[2] / 255f, new_colorRGBA[3] / 255f);
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
	
	public void hvspeed_position(GameObject obj, Vector2 hvspeed, bool round = true){
		/*
		Atualiza posição de um objeto e retorna hvspeed.
		
		Parâmetros:
			obj (GameObject) - o objeto
			hvspeed (Vector2) - velocidade de deslocamento horizontal e vertical
		
		Retorno:
			Posição de objeto atualizada
		*/
		
		Vector3 new_position = new Vector3(
			obj.transform.position.x + hvspeed.x,
			obj.transform.position.y + hvspeed.y,
			obj.transform.position.z
		);
		if(round){
			obj.transform.position = roundVector3(new_position);
		}else{
			obj.transform.position = new_position;
		}
	}
	
	public bool InputKey(string key, int mode){
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
		}else if(key.Length == 1){
			if(mode == 0){
				choice = Input.GetKey(key);
			}else if(mode == 1){
				choice = Input.GetKeyDown(key);
			}	
		}

		return choice;
	}
	
	public void debug_drawRay(Rect rect){
		/*
		*/
		
		Color color = Color.yellow;
		Debug.DrawRay(new Vector3(rect.x, rect.y, 0), new Vector2(rect.width, rect.height), color);
	}
	
	public void debug_drawRect(Rect rect, Color color){
		/*
		Desenha retângulo para debug.
		
		Parâmetros:
			rect (Rect) - dados do retângulo
			color (Color) - cor do retângulo
			
		Retorno:
			Retângulo desenhado na tela
		*/
		
		Debug.DrawRay(new Vector3(rect.x, rect.y, 0), new Vector2(rect.width, 0), color); // up
		Debug.DrawRay(new Vector3(rect.x, rect.y-rect.height, 0), new Vector2(rect.width, 0), color); // down
		Debug.DrawRay(new Vector3(rect.x, rect.y, 0), new Vector2(0, -rect.height), color); // left
		Debug.DrawRay(new Vector3(rect.x+rect.width, rect.y, 0), new Vector2(0, -rect.height), color); // right
	}
	
	public bool CheckCollision_BoxCollider2D(GameObject obj, string[] arr_colliderName, float adjust = 0){
		/*
		Colisão de quatro direções para objeto
		
		Parâmetros:
			obj (GameObject) - o objeto
			arr_colliderName (string[]) - nome do objeto colisor
			
		Retorno:
			Veradeiro ou falso para colisão
		*/
		
		bool collided = false;
		Vector3 origin = new Vector3(
			obj.transform.position.x + obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y + obj.GetComponent<BoxCollider2D>().offset.y,
			obj.transform.position.z
		);
		Vector2 size = new Vector2(
			obj.GetComponent<BoxCollider2D>().size.x + adjust, 
			obj.GetComponent<BoxCollider2D>().size.y + adjust
		);
		float angle = 0;
		Vector2 direction = new Vector2(0, 0);
		float distance = 0;
		RaycastHit2D[] boxCast = Physics2D.BoxCastAll(origin, size, angle, direction, distance);

		foreach(var hit in boxCast){
			foreach(var name in arr_colliderName){
				if(hit.collider.gameObject.name.StartsWith(name) 
				&& hit.collider.gameObject.name != obj.transform.name){
					collided = true;
					break;
				}
			}
		}
		
		return collided;
	}
	
	public bool checkCollision_CircleCollider2D(GameObject obj, string[] arr_colliderName, float adjust = 0){
		bool collided = false;
		Vector3 origin = new Vector3(
			obj.transform.position.x + obj.GetComponent<CircleCollider2D>().offset.x, 
			obj.transform.position.y + obj.GetComponent<CircleCollider2D>().offset.y,
			obj.transform.position.z
		);
		float radius = obj.GetComponent<CircleCollider2D>().radius;
		Vector2 direction = new Vector2(0, 0);
		float distance = 0;
		RaycastHit2D[] circleCast = Physics2D.CircleCastAll(origin, radius, direction, distance);
		
		foreach(var hit in circleCast){
			foreach(var name in arr_colliderName){
				if(hit.collider.gameObject.name.StartsWith(name) 
				&& hit.collider.gameObject.name != obj.transform.name){
					collided = true;
					break;
				}
			}
		}
		
		return collided;
	}
	
	public bool checkCollision_CapsuleCollider2D(GameObject obj, string[] arr_colliderName, float adjust = 0){
		bool collided = false;
		Vector2 origin = new Vector2(
			obj.transform.position.x + obj.GetComponent<CapsuleCollider2D>().offset.x, 
			obj.transform.position.y + obj.GetComponent<CapsuleCollider2D>().offset.y
		);
		Vector2 size = new Vector2(
			obj.GetComponent<CapsuleCollider2D>().size.x + adjust, 
			obj.GetComponent<CapsuleCollider2D>().size.y + adjust
		);
		CapsuleDirection2D capsuleDirection = obj.GetComponent<CapsuleCollider2D>().direction;
		float angle = 0;
		Vector2 direction = Vector2.zero;
		float distance = 0;
		RaycastHit2D[] capsuleCast = Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance);
		
		foreach(var hit in capsuleCast){
			foreach(var name in arr_colliderName){
				if(hit.collider.gameObject.name.StartsWith(name) 
				&& hit.collider.gameObject.name != obj.transform.name){
					collided = true;
					break;
				}
			}
		}
		
		return collided;
	}

	public void load_scene(string name){
		/*
		Carrega uma cena.
		
		Parâmetros:
			name (string) - nome da cena

		Retorno:
			Cena carregada
		*/
		
		if(name == ""){
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}else{
			SceneManager.LoadScene(name);
		}
	}

	public void save_game(Save save){
		/*
		Cria/atualiza arquivo de save do script/classe Save.
		
		Parâmetros:
			save (Save) - classe Save
			
		Retorno:
			Retorna arquivo de save atualizado
		*/
		
		string path = Application.persistentDataPath; // C:/Users/Adammo/AppData/LocalLow/DefaultCompany/uProject_runRunChamp
		FileStream file = File.Create(path + "/saveFile.save");
		BinaryFormatter BF = new BinaryFormatter();
		BF.Serialize(file, save);
		file.Close();
	}
	
	public Save load_game(){
		/*
		Carrega dados do arquivo de save e retorna em uma variável.
		
		Parâmetros:
			Nenhum
			
		Retorno:
			Variável do tipo Save
		*/
		
		string path = Application.persistentDataPath;
		FileStream file;
		BinaryFormatter BF = new BinaryFormatter();
		
		if(File.Exists(path + "/saveFile.save")){
			file = File.Open(path + "/saveFile.save", FileMode.Open);
			Save load = (Save)BF.Deserialize(file);
			file.Close();
		
			return load;
		}
		
		return null;
	}
	
	public void set_animation(GameObject obj, string[] animation, float frame){
		/*
		Toca uma nova animação se animation[0] for diferente de animation[1].
		
		Parâmetros:
			obj (GameObject) - o objeto
			animation (string[]) - animação atual e posterior
			frame (float) - frame inicial, escala de 0 a 1
			
		Retorno:
			Inicia nova animação
		*/
		
		if(animation[0] != animation[1]){
			Animator animator = obj.GetComponent<Animator>();
			animator.Play(animation[1], 0, frame);
			animation[0] = animation[1];
		}
	}
	
	public void replaceColor(GameObject obj, string propertyName, Vector4 new_colorRGBA){
		/*
		Troca cor da propriedade do material.
		
		Parâmetros:
			obj (GameObject) - o objeto
			propertyName (string) - nome da propriedade do material
			new_colorRGBA (Vector4) - a nova cor no formato RGBA 0-255
			
		Retorno:
			Atualiza cor da propriedade
		*/
		
		SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
		Color new_color = new Vector4(new_colorRGBA[0] / 255f, new_colorRGBA[1] / 255f, new_colorRGBA[2] / 255f, new_colorRGBA[3] / 255f);
		spriteRenderer.material.SetColor(propertyName, new_color);
	}
	
	public void set_zOrder(GameObject obj, int zOrder){
		/*
		Altera a o número da camada do objeto
		
		Parâmetros:
			obj (GameObject) - o objeto
			int (zOrder) - número da camada
		
		Retorno:
			Número da camada atualizado
		*/
		
		SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
		if(spriteRenderer.sortingOrder != zOrder){
			spriteRenderer.sortingOrder = zOrder;
		}
	}
	
	public void debug_drawLine(Vector3 origin, Vector2 direction, Color color){
		/*
		Desenha uma linha de debug.
		
		Parâmetros:
			origin (Vector3) - ponto de origem da linha
			direction (Vector2) - direção da linha
			color (Color) - cor
		
		Retorno:
			Linha desenhanda
		*/
		
		Debug.DrawRay(origin, direction, color);
	}
	
	public Vector3 roundVector3(Vector3 vector3){
		/*
		Arredonda com Round uma variável do tipo Vector3.
		
		Parâmetros:
			vector3 (Vector3) - o Vector3
		
		Retorno:
			Valor arredondado
		*/
		
		Vector3 new_vector3 = new Vector3(Mathf.Round(vector3.x), Mathf.Round(vector3.y), Mathf.Round(vector3.z));
		
		return new_vector3;
	}

	public RaycastHit2D[] boxColision_getHits(GameObject obj, Vector2 hvspeed, string colliderName){
		/*
		Colisão em caixa para captura de RaycastHit2D[] do objeto colisor
		
		Parâmetros:
			obj (GameObject) - o objeto
			hvspeed (Vector2) - velocidade horizontal e vertical atual
			colliderName (string) - nome do objeto colisor
		
		Retorno:
			Retorna RaycastHit2D[] do objeto colisor
		*/
		
		Vector3 origin = new Vector3(
			obj.transform.position.x+obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y+obj.GetComponent<BoxCollider2D>().offset.y, 
			obj.transform.position.z
		);
		float adjustment = 0.1f;
		Vector2 size = new Vector2(obj.GetComponent<BoxCollider2D>().size[0]-adjustment, obj.GetComponent<BoxCollider2D>().size[1]-adjustment);
		float angle = 0;
		Vector2 h_direction = new Vector2(Sign(hvspeed[0]), 0);
		Vector2 v_direction = new Vector2(0, Sign(hvspeed[1]));
		Vector2 d_direction = new Vector2(Sign(hvspeed[0]), Sign(hvspeed[1]));
		float h_distance = Mathf.Abs(hvspeed[0]);
		float v_distance = Mathf.Abs(hvspeed[1]);
		float d_distance = Mathf.Max(Mathf.Abs(hvspeed[0]), Mathf.Abs(hvspeed[1]));
		RaycastHit2D[] h_boxCast = Physics2D.BoxCastAll(origin, size, angle, h_direction, h_distance);
		RaycastHit2D[] v_boxCast = Physics2D.BoxCastAll(origin, size, angle, v_direction, v_distance);
		RaycastHit2D[] d_boxCast = Physics2D.BoxCastAll(origin, size, angle, d_direction, d_distance);
		Color color = Color.yellow;
		bool[] collided = {false, false, false};
		RaycastHit2D[] hits = {h_boxCast[0], v_boxCast[0], d_boxCast[0]};

		foreach(var hit in h_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				color = Color.red;
				hits[0] = hit;
				collided[0] = true;
				break;
			}
		}
		foreach(var hit in v_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				color = Color.red;
				hits[1] = hit;
				collided[1] = true;
				break;
			}
		}
		foreach(var hit in v_boxCast){
			if(collided[0] == false
			&& collided[1] == false){
				if(hit.collider.gameObject.name.StartsWith(colliderName)){
					color = Color.red;
					hits[2] = hit;
					collided[2] = true;
					break;
				}
			}
		}
		debug_drawRect(
			new Rect(
				obj.GetComponent<BoxCollider2D>().bounds.center.x-(size[0]/2), 
				obj.GetComponent<BoxCollider2D>().bounds.center.y+(size[1]/2), 
				size[0], 
				size[1]
			), 
			color
		);
		
		return hits;
	}
	
	public float Sign(float number){
		/*
		Se number é positivo, negativo ou nenhum dos dois e retorna 1, -1 ou 0, respectivamente.
		
		Parâmetros:
			number (float) - o valor
			
		Retorno:
			number transformado
		*/
		
		float new_number = 0;
		if(number > 0){new_number = 1;}
		if(number == 0){new_number = 0;}
		if(number < 0){new_number = -1;}
		
		return new_number;
	}
	
	public Vector2 boxColision_wall(GameObject obj, Vector2 hvspeed, string colliderName, bool scale = false){
		/*
		Colisão de quatro direções para parede
		
		Parâmetros:
			obj (GameObject) - o objeto
			hvspeed (Vector2) - velocidade horizontal e vertical atual
			colliderName (string) - nome do objeto colisor
			scale (bool) - 
			
		Retorno:
			hvspeed atualizado
		*/
		
		Vector3 origin = new Vector3(
			obj.transform.position.x+obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y+obj.GetComponent<BoxCollider2D>().offset.y, 
			obj.transform.position.z
		);
		Vector3 d_origin = new Vector3(
			obj.transform.position.x+obj.GetComponent<BoxCollider2D>().offset.x+((obj.GetComponent<BoxCollider2D>().size.x/2)*Sign(hvspeed[0])),
			obj.transform.position.y+obj.GetComponent<BoxCollider2D>().offset.y+((obj.GetComponent<BoxCollider2D>().size.y/2)*Sign(hvspeed[1])), 
			obj.transform.position.z
		);
		float adjustment = 0.1f;
		Vector2 size = new Vector2(obj.GetComponent<BoxCollider2D>().size[0]-adjustment, obj.GetComponent<BoxCollider2D>().size[1]-adjustment);
		if(scale == true){size = new Vector2(obj.transform.localScale.x-adjustment, obj.transform.localScale.y-adjustment);}
		float angle = 0;
		Vector2 h_direction = new Vector2(Sign(hvspeed[0]), 0);
		Vector2 v_direction = new Vector2(0, Sign(hvspeed[1]));
		Vector2 d_direction = new Vector2(Sign(hvspeed[0]), Sign(hvspeed[1]));
		float h_distance = Mathf.Abs(hvspeed[0]);
		float v_distance = Mathf.Abs(hvspeed[1]);
		float d_distance = Mathf.Max(Mathf.Abs(hvspeed[0]), Mathf.Abs(hvspeed[1]));
		RaycastHit2D[] h_boxCast = Physics2D.BoxCastAll(origin, size, angle, h_direction, h_distance);
		RaycastHit2D[] v_boxCast = Physics2D.BoxCastAll(origin, size, angle, v_direction, v_distance);
		RaycastHit2D[] d_rayCast = Physics2D.RaycastAll(d_origin, d_direction, d_distance);
		bool[] collided = new bool[]{false, false};
		Color color = Color.yellow;

		foreach(var hit in h_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				if(hit.collider.gameObject.name != obj.transform.name){
					color = Color.red;
					hvspeed[0] = Sign(hvspeed[0]) * (hit.distance-adjustment);
					collided[0] = true;
					break;
				}
			}
		}
		foreach(var hit in v_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				if(hit.collider.gameObject.name != obj.transform.name){
					color = Color.red;
					hvspeed[1] = Sign(hvspeed[1]) * (hit.distance-adjustment);
					collided[1] = true;
					break;
				}
			}
		}
		foreach(var hit in d_rayCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName) && collided[0] == false && collided[1] == false){
				if(hit.collider.gameObject.name != obj.transform.name){
					if(Random.Range(0, 2) == 0){
						hvspeed[0] = 0;//Sign(hvspeed[0]) * (hit.distance-adjustment);
					}else{
						hvspeed[1] = 0;//Sign(hvspeed[1]) * (hit.distance-adjustment);
					}
					break;
				}
			}
		}
		debug_drawRect(
			new Rect(
				obj.GetComponent<BoxCollider2D>().bounds.center.x-(size[0]/2), 
				obj.GetComponent<BoxCollider2D>().bounds.center.y+(size[1]/2), 
				size[0], 
				size[1]
			), 
			color
		);
		
		return hvspeed;
	}

	public void pushObject(GameObject obj, Vector2 hvspeed){
		/*
		Empurrar um objeto.
		
		Parâmetros:
			obj (GameObject) - o objeto
			hvspeed (Vector2) - velocidade horizontal e vertical atual
		
		Retorno:
			Objeto empurrado
		*/
		
		Vector3 new_position = new Vector3(
			obj.transform.position.x + hvspeed[0],
			obj.transform.position.y + hvspeed[1],
			obj.transform.position.z
		);
		set_position(obj, new_position);
	}

	public void RestartScene(){
		/*
		*/
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void rotate2D(GameObject obj, int direction, float speed){
		/*
		Rotaciona um objeto 2D com sentido e velocidade definidos.
		
		Parâmetros:
			obj (GameObject): Objeto fornecido
			direction (int): Sentido que o objeto irá rotacionar, horário (1) ou anti-horário (-1)
			speed (float): Velocidade de rotação do objeto
		*/
		
		Vector3 rotation = obj.transform.eulerAngles;
		if(direction == 1){
			rotation.z -= speed;
		}else if(direction == -1){
			rotation.z += speed;
		}
		obj.transform.eulerAngles = rotation;
	}
	
	public void Rotate(GameObject obj, int direction, float speed){
		Vector3 angle = obj.transform.eulerAngles;
		
		angle.z += direction * speed;
		obj.transform.eulerAngles = angle;
	}
	
	public void RotateY(GameObject obj, int direction, float speed){
		Vector3 angle = obj.transform.eulerAngles;
		
		angle.y += direction * speed;
		obj.transform.eulerAngles = angle;
	}
	
	public void set_angle(GameObject obj, float new_angle){
		/*
		*/
		
		Vector3 angle = obj.transform.eulerAngles;
		angle.z = new_angle;
		obj.transform.eulerAngles = angle;
	}

	public void followObject(GameObject obj, GameObject follow_obj, Vector3 difference){
		/*
		*/
		
		Vector3 adjustment = follow_obj.transform.position + difference;
		set_position(obj, adjustment);
	}
	
	public void followObject2D(GameObject obj, GameObject follow_obj, Vector2 difference){
		/*
		*/
		
		Vector3 adjustment = new Vector3(
			follow_obj.transform.position.x + difference.x, 
			follow_obj.transform.position.y + difference.y, 
			obj.transform.position.z
		);
		set_position(obj, adjustment);
	}

	public GameObject get_child(GameObject obj, int index){
		/*
		*/
		
		GameObject child = obj.transform.GetChild(index).gameObject;
		
		return child;
	}

	public void sprite2D_flip(SpriteRenderer SR, int side){
		/*
		Defini a direção para qual um SpriteRenderer deve refletir.
		
		Parâmetros:
			SR (SpriteRenderer): Sprite fornecido
			side (int): Direção para qual a SpriteRenderer deve refletir
		*/
		
		if(side == 1){
			if(SR.flipX == true){
				SR.flipX = false;
			}
		}
		if(side == -1){
			if(SR.flipX == false){
				SR.flipX = true;
			}
		}
	}

	public void camera_betweenTwoObjects(GameObject cam, GameObject obj_1, GameObject obj_2, Vector3 difference){
		/*
		*/
		
		Vector3 position = cam.transform.position;
		position.x = (obj_1.transform.position.x + obj_2.transform.position.x) / 2;
		position.y = (obj_1.transform.position.y + obj_2.transform.position.y) / 2;
		position.z = cam.transform.position.z;
		position += difference;
		set_position(cam, position);
	}
	
	public void camera_betweenTwoObjects_bounds(GameObject cam, GameObject obj_1, GameObject obj_2, Vector3 difference, Vector2 roomSize, GameObject bounds){
		/*
		*/
		
		Vector3 position = cam.transform.position;
		position.x = (obj_1.transform.position.x + obj_2.transform.position.x) / 2;
		position.y = (obj_1.transform.position.y + obj_2.transform.position.y) / 2;
		position.z = cam.transform.position.z;
		position += difference;
		float x1 = (-(roomSize.x / 2) + difference.x) + (bounds.transform.localScale.x / 2);
		float x2 = ((roomSize.x / 2) + difference.x) - (bounds.transform.localScale.x / 2);
		float y1 = (-(roomSize.y / 2) + difference.y) + (bounds.transform.localScale.y / 2);
		float y2 = ((roomSize.y / 2) + difference.y) - (bounds.transform.localScale.y / 2);
		if(position.x < x1){position.x = x1;}
		if(position.x > x2){position.x = x2;}
		if(position.y < y1){position.y = y1;}
		if(position.y > y2){position.y = y2;}
		
		set_position(cam, position);
	}

	public void Set_ScaleXY(GameObject obj, Vector2 new_scale){
		/*
		*/
		
		float x = new_scale.x;
		float y = new_scale.y;
		float z = obj.transform.localScale.z;
		obj.transform.localScale = new Vector3(x, y, z);
	}

	public void set_boxCollider2dSize(GameObject obj, Vector2 size, Vector2 offset){
		/*
		*/
		
		obj.GetComponent<BoxCollider2D>().size = size;
		obj.GetComponent<BoxCollider2D>().offset = offset;
	}

	public GameObject Load_GameObject(string name){
		/*
		*/
		
		GameObject clone = Resources.Load<GameObject>(name);
		
		return clone;
	}
	
	public void InstantiateObject(GameObject obj, Vector3 position){
		/*
		*/
		
		Instantiate(obj, position, Quaternion.identity);
	}

	public void DestroyObject(GameObject obj, bool parent = false){
		/*
		*/
		
		if(parent){
			if(obj.transform.parent != null){
				Destroy(obj.transform.parent.gameObject);
			}else{
				Destroy(obj);
			}
		}else{
			Destroy(obj);
		}
	}

	public void Set_Opacity(GameObject obj, float opacity){
		/*
		Defini a opacidade de um SpriteRenderer.
		
		Parâmetros:
			obj (GameObject): Objeto fornecido
			opacity (float): Opacidade de 0 a 100
		*/
	
		Color color = obj.GetComponent<SpriteRenderer>().color;
		color.a = opacity;
		obj.GetComponent<SpriteRenderer>().color = color;
	}

	public float distance_betweenTwoObjects(GameObject obj_1, GameObject obj_2){
		/*
		*/
		
		float distance = Vector3.Distance(obj_1.transform.position, obj_2.transform.position);
		
		return distance;
	}

	public void set_sortingOrder(GameObject obj, int number){
		/*
		*/
		
		obj.GetComponent<SpriteRenderer>().sortingOrder = number;
	}
	
	public void boxColision_set_sortingOrder(GameObject obj, string colliderName){
		/*
		*/
		
		Vector3 origin = new Vector3(
			obj.transform.position.x+obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y+obj.GetComponent<BoxCollider2D>().offset.y,
			obj.transform.position.z
		);
		float adjustment = 0.1f;
		Vector2 size = new Vector2(obj.GetComponent<BoxCollider2D>().size[0]-adjustment, obj.GetComponent<BoxCollider2D>().size[1]-adjustment);
		float angle = obj.transform.eulerAngles.z;
		Vector2 direction = new Vector2(0, 0);
		float distance = 0;
		RaycastHit2D[] boxCast = Physics2D.BoxCastAll(origin, size, angle, direction, distance);

		foreach(var hit in boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				GameObject other = get_child(hit.collider.gameObject, 0);
				if(obj.transform.position.y > (other.transform.position.y + 12)){
					set_sortingOrder(obj, 0);
				}else{
					set_sortingOrder(obj, 2);
				}
			}
			//break;
		}
	}

	public List<GameObject> boxColision_getObject(GameObject obj, string colliderName){
		/*
		*/
		
		List<GameObject> objects = new List<GameObject>();
		Vector3 origin = new Vector3(
			obj.transform.position.x+obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y+obj.GetComponent<BoxCollider2D>().offset.y,
			obj.transform.position.z
		);
		float adjustment = 0.1f;
		Vector2 size = new Vector2(obj.GetComponent<BoxCollider2D>().size[0]-adjustment, obj.GetComponent<BoxCollider2D>().size[1]-adjustment);
		float angle = obj.transform.eulerAngles.z;
		Vector2 direction = new Vector2(0, 0);
		float distance = 0;
		RaycastHit2D[] boxCast = Physics2D.BoxCastAll(origin, size, angle, direction, distance);
		Color color = Color.yellow;

		foreach(var hit in boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName)){
				color = Color.red;
				objects.Add(hit.collider.gameObject);
			}
		}
		
		return objects;
	}

	public Vector2 checkDistance_BoxCollider2D(GameObject obj, Vector2 distance, string colliderName, float adjust = 0){
		/*
		*/
		
		Vector2 the_distance = distance;
		Vector3 origin = new Vector3(
			obj.transform.position.x + obj.GetComponent<BoxCollider2D>().offset.x, 
			obj.transform.position.y + obj.GetComponent<BoxCollider2D>().offset.y, 
			obj.transform.position.z
		);
		Vector2 size = new Vector2(
			obj.GetComponent<BoxCollider2D>().size.x + adjust, 
			obj.GetComponent<BoxCollider2D>().size.y + adjust
		);
		float angle = obj.transform.eulerAngles.z;
		Vector2 h_direction = new Vector2(Sign(distance.x), 0);
		Vector2 v_direction = new Vector2(0, Sign(distance.y));
		Vector2 d_direction = new Vector2(Sign(distance.x), Sign(distance.y));
		float h_distance = Mathf.Abs(distance.x);
		float v_distance = Mathf.Abs(distance.y);
		float d_distance = Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.y));
		RaycastHit2D[] h_boxCast = Physics2D.BoxCastAll(origin, size, angle, h_direction, h_distance);
		RaycastHit2D[] v_boxCast = Physics2D.BoxCastAll(origin, size, angle, v_direction, v_distance);
		RaycastHit2D[] d_boxCast = Physics2D.BoxCastAll(origin, size, angle, d_direction, d_distance);
		Color color = Color.yellow;
		bool[] collided = new bool[]{false, false};
		
		foreach(var hit in h_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName) 
			&& hit.collider.gameObject.name != obj.transform.name){
				color = Color.red;
				the_distance.x = h_direction.x * (hit.distance + adjust);
				collided[0] = true;
				break;
			}
		}
		foreach(var hit in v_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName) 
			&& hit.collider.gameObject.name != obj.transform.name){
				color = Color.red;
				the_distance.y = v_direction.y * (hit.distance + adjust);
				collided[1] = true;
				break;
			}
		}
		foreach(var hit in d_boxCast){
			if(hit.collider.gameObject.name.StartsWith(colliderName) 
			&& hit.collider.gameObject.name != obj.transform.name
			&& collided[0] == false
			&& collided[1] == false){		
				if(Random.Range(0, 2) == 0){
					the_distance.x = 0;
				}else{
					the_distance.y = 0;
				}
				break;
			}
		}

		debug_drawRect(
			new Rect(
				obj.GetComponent<BoxCollider2D>().bounds.center.x - (size.x / 2), 
				obj.GetComponent<BoxCollider2D>().bounds.center.y + (size.y / 2), 
				size.x, 
				size.y
			), 
			color
		);
		
		return the_distance;
	}

	public float[] shake_object(GameObject obj, float[] shake_values, float force = 1){
		/*
		*/
		
		Vector2 direction = new Vector2(shake_values[0], shake_values[1]);
		Vector3 position = obj.transform.position;
		float timer = shake_values[2];
		
		if(timer == 0){
			position = new Vector3(position.x + (direction.x * force), position.y + (direction.y * force), position.z);
			set_position(obj, position);
			if(direction.x != 0 && direction.x == 1){direction.x = -1;}
			else if(direction.x != 0 && direction.x == -1){direction.x = 1;}
			if(direction.y != 0 && direction.y == 1){direction.y = -1;}
			else if(direction.y != 0 && direction.y == -1){direction.y = 1;}
		}
		timer += shake_values[4];
		if(timer == shake_values[3]){
			timer = 0;
		}
		
		float[] new_shakeValues = new float[]{
			direction.x, direction.y,
			timer, shake_values[3], shake_values[4]
		};
		return new_shakeValues;
	}

	public void flip_sprite(SpriteRenderer SR){
		/*
		*/
		
		if(SR.flipX == false){
			SR.flipX = true;
		}else{
			SR.flipX = false;
		}
	}

	public int[] shuffle_array(int[] array){
		/*
		*/
		
		int size = array.Length;
		while(size > 1){
			int x = Random.Range(1, size);
			int y = array[x];
			array[x] = array[0];
			array[0] = y;
			size--;
		}
		
		return array;
	}
	
	public bool go_target(GameObject obj, Vector2 target, float speed = 1){
		/*
		*/
		
		bool goal = false;
		Vector2 new_position = new Vector2(obj.transform.position.x, obj.transform.position.y);
		
		if(new_position.x < target.x){
			new_position.x += speed;
			if(new_position.x > target.x){new_position.x = target.x;}
		}
		if(new_position.x > target.x){
			new_position.x -= speed;
			if(new_position.x < target.x){new_position.x = target.x;}
		}
		if(new_position.y < target.y){
			new_position.y += speed;
			if(new_position.y > target.y){new_position.y = target.y;}
		}
		if(new_position.y > target.y){
			new_position.y -= speed;
			if(new_position.y < target.y){new_position.y = target.y;}
		}
		obj.transform.position = new_position;
		if(new_position == target){goal = true;}
		
		return goal;
	}
	
	public void set_layer(GameObject obj, string layer){
		/*
		*/
		
		obj.GetComponent<SpriteRenderer>().sortingLayerName = layer;
	}
}
