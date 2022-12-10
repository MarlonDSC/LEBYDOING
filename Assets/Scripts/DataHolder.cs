using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun; 
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class DataHolder : MonoBehaviourPunCallbacks
{
	
	[Header("Photon Data")]
	private ExitGames.Client.Photon.Hashtable CustomPlayer = new ExitGames.Client.Photon.Hashtable();
	public GameObject GeneroBtns,AllBtns; 
	
	[Header("Botones")]
	string botones;
	public Button  BgeneroF,BgeneroM , BPiel, BOjos, BColorPelo,BPelo, BCamisa, BChaqueta, BPantalones, BZapato, btnSave, btnGo; 
	
	[Header("DATA")]
	public GameObject HombreGB;
	public GameObject MujerGB; 
	public int Lgenero, LPiel, LOjos, LColorPelo,LPelo, LCamisa, LChaqueta, LPantalones, LZapato; 
	
	
	[Header("PLAYER DATA")]
	
	//public int Genero;
	
	////////////////////////////////////////////////////////////////

	[Header("HOMBRE DATA")]
	public SkinnedMeshRenderer pielCabezaHombre; 
	public SkinnedMeshRenderer pielBrazoHombre,SueterHombre,PantalonesHombre,ZapatosHombre;
	//quizas lo de abajo se borre
	
	public Material[] Color_PIEL_Hombre,Color_OjosHombre, Camisa_Hombre, 
	Color_Sueter_Hombre, Color_Pantalones_Hombre, Color_Zapatos_Hombre;
	
	[Header("HOMBRE GameObjects")]
	public GameObject[] Pelo_Hombre; 
	public GameObject Sueter_Hombre; 

	////////////////////////////////////////////////////////////////
	
	[Header("MUJER DATA")]
	public SkinnedMeshRenderer pielCabezaMujer; 
	public SkinnedMeshRenderer pielBrazoMujer,SueterMujer,PantalonesMujer,ZapatosMujer;
	//quizas lo de abajo se borre
	
	public Material[] Color_PIEL_Mujer,Color_OjosMujer, Camisa_Mujer, 
	Color_Sueter_Mujer, Color_Pantalones_Mujer, Color_Zapatos_Mujer;
	
	[Header("HOMBRE GameObjects")]
	public GameObject[] Pelo_Mujer; 
	public GameObject Sueter_Mujer; 
	
	
	////////////////////////////////////////////////////////////////

	//valores de puente
	Material[] H_CabezaMat,H_PielMat,H_OjosMat, H_SueterMat,H_PantsMat,H_Zapatos;

	CargandoManager mgr;
	InitialConnect init;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		AllBtns.SetActive(false);
		GeneroBtns.SetActive(true);
	}
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		init = FindObjectOfType<InitialConnect>();
		Lgenero = Random.Range(0,1);
		GeneroSel(false);
		
		
		BgeneroF.onClick.AddListener(()=> { Lgenero = 1; GeneroSel(true);});
		BgeneroM.onClick.AddListener(()=> { Lgenero = 0; GeneroSel(true);});
		

		BPiel.onClick.AddListener(()=> cambioAtributo("Piel"));
		BOjos.onClick.AddListener(()=> cambioAtributo("Ojos"));
		BColorPelo.onClick.AddListener(()=> cambioAtributo("Colorpelo"));
		BPelo.onClick.AddListener(()=> cambioAtributo("TipoPelo"));
		BCamisa.onClick.AddListener(()=> cambioAtributo("Camisa"));
		BChaqueta.onClick.AddListener(()=> cambioAtributo("Sueter"));
		BPantalones.onClick.AddListener(()=> cambioAtributo("Pantalones"));
		BZapato.onClick.AddListener(()=> cambioAtributo("Zapatos"));

		btnSave.onClick.AddListener(()=> GeneroSel(false));
		btnGo.onClick.AddListener(ChangeScene);

	}
	
	void GeneroSel(bool boton)
	{
		//seteo de puentes
		//es una pana
		if(boton)
		{
			GeneroBtns.SetActive(false);
			AllBtns.SetActive(true);
		}
		else
		{
			GeneroBtns.SetActive(true);
			AllBtns.SetActive(false);
		}
		
		
		if(Lgenero == 1)
		{
			HombreGB.SetActive(false);
			MujerGB.SetActive(true);
			H_CabezaMat = pielCabezaMujer.materials;
			H_PielMat = pielBrazoMujer.materials;
			H_PantsMat = PantalonesMujer.materials;
			H_Zapatos = ZapatosMujer.materials;
		}	
		else // es un pana
		{
			HombreGB.SetActive(true);
			MujerGB.SetActive(false);
		
			H_CabezaMat = pielCabezaHombre.materials;
			H_PielMat = pielBrazoHombre.materials;
			H_PantsMat = PantalonesHombre.materials;
			H_Zapatos = ZapatosHombre.materials;
			
		}
		
	}
	
	
	
	void cambioAtributo(string Atributo)
	{
		
		
		
		if(Lgenero==0){
		//switch de atributos masculinos
		switch (Atributo)
		{
		
				case "Piel":
				//sumo y confirmo que no me pasare del array
				LPiel++;
				if(LPiel >= Color_PIEL_Hombre.Length-1)
				{
					LPiel=0;	
				}
					//cambio pie en cabezas
					H_CabezaMat[1] = Color_PIEL_Hombre[LPiel];
					pielCabezaHombre.materials = H_CabezaMat; 
					//cambio piel en todo el body
					H_PielMat[0] = Color_PIEL_Hombre[LPiel];
					pielBrazoHombre.materials = H_PielMat; 
					
					//cambio la piel en el sueter
					H_SueterMat = SueterHombre.materials;
					H_SueterMat[0] = Color_PIEL_Hombre[LPiel];
					H_SueterMat[1] = Color_Sueter_Hombre[LChaqueta];
					SueterHombre.materials = H_SueterMat;	
					
					break;
				
				case "Ojos":
					//sumo y confirmo que no me pasare del array
					LOjos++;
					if(LOjos >= Color_OjosHombre.Length-1)
					{
						LOjos=0;	
					}
					//cambio piel en cabezas
					H_CabezaMat[0] = Color_OjosHombre[LOjos];
					pielCabezaHombre.materials = H_CabezaMat; 
					break;
					 
				case "Colorpelo":
					//sumo y confirmo que no me pasare del array
					LColorPelo++;
					if(LColorPelo >= 4)
					{
						LColorPelo=0;	
					}
					//cambio color en cabellos
					if(LColorPelo==0)
					{
					foreach (GameObject go in Pelo_Hombre)
					{
						go.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
					}
					}
					
					if(LColorPelo==1)
					{
						foreach (GameObject go in Pelo_Hombre)
						{
							go.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
						}
					}
					
					if(LColorPelo==2)
					{
						foreach (GameObject go in Pelo_Hombre)
						{
							go.GetComponent<SkinnedMeshRenderer>().material.color = Color.green;
						}
					}
					
					if(LColorPelo==3)
					{
						foreach (GameObject go in Pelo_Hombre)
						{
							go.GetComponent<SkinnedMeshRenderer>().material.color = Color.yellow;
						}
					}

					break;
					
					
				case "TipoPelo":
					//sumo y confirmo que no me pasare del array
					LPelo++;
					if(LPelo >= Pelo_Hombre.Length-1)
					{
						LPelo=0;	
					}
					foreach (GameObject item in Pelo_Hombre)
					{
						item.SetActive(false);
					}
					Pelo_Hombre[LPelo].SetActive(true);
					break;
					
				case "Camisa":
					//sumo y confirmo que no me pasare del array
					LCamisa++;
					if(LCamisa >= Camisa_Hombre.Length-1)
					{
						LCamisa=0;	
					}
					//cambio piel en cabezas
					H_PielMat[1] = Camisa_Hombre[LCamisa];
					pielBrazoHombre.materials = H_PielMat; 
					break;
					
				case "Sueter":
					//sumo y confirmo que no me pasare del array
					LChaqueta++;
					if(LChaqueta >= Color_Sueter_Hombre.Length-1)
					{
						LChaqueta=0;	
					}
					if(LChaqueta==0){
						Sueter_Hombre.SetActive(false);
					}
					else
					{
						Sueter_Hombre.SetActive(true);
						H_SueterMat = SueterHombre.materials;
						H_SueterMat[0] = Color_PIEL_Hombre[LPiel];
						H_SueterMat[1] = Color_Sueter_Hombre[LChaqueta];
						SueterHombre.materials = H_SueterMat;	
					}
					break;
					
				case "Pantalones":
					//sumo y confirmo que no me pasare del array
					LPantalones++;
					if(LPantalones >= Camisa_Hombre.Length-1)
					{
						LPantalones=0;	
					}
					//cambio piel en cabezas
					H_PantsMat[0] = Color_Pantalones_Hombre[LPantalones];
					PantalonesHombre.materials = H_PantsMat; 
					break;
					
				case "Zapatos":
					//sumo y confirmo que no me pasare del array
					LZapato++;
					if(LZapato >= Color_Zapatos_Hombre.Length-1)
					{
						LZapato=0;	
					}
					//cambio piel en cabezas
					H_Zapatos[0] = Color_Zapatos_Hombre[LZapato];
					ZapatosHombre.materials = H_Zapatos; 
					break;
			}
		}
		//si es una mujer... 
			else{
			
				cambioAtributoMujer(Atributo);
				
		}
		
	}
	
	
	
	
	
	
	
	void cambioAtributoMujer(string Atributo)
	{
		
		switch (Atributo)
		{
		
		case "Piel":
			//sumo y confirmo que no me pasare del array
			LPiel++;
			if(LPiel >= Color_PIEL_Mujer.Length-1)
			{
				LPiel=0;	
			}
			//cambio pie en cabezas
			H_CabezaMat[0] = Color_PIEL_Mujer[LPiel];
			pielCabezaMujer.materials = H_CabezaMat; 
			//cambio piel en todo el body
			H_PielMat[0] = Color_PIEL_Mujer[LPiel];
			pielBrazoMujer.materials = H_PielMat; 
			break;
				
		case "Ojos":
			//sumo y confirmo que no me pasare del array
			LOjos++;
			if(LOjos >= Color_OjosMujer.Length-1)
			{
				LOjos=0;	
			}
			//cambio piel en cabezas
			H_CabezaMat[3] = Color_OjosMujer[LOjos];
			pielCabezaMujer.materials = H_CabezaMat; 
			break;
					 
		case "Colorpelo":
			//sumo y confirmo que no me pasare del array
			LColorPelo++;
			if(LColorPelo >= 4)
			{
				LColorPelo=0;	
			}
			//cambio color en cabellos
			if(LColorPelo==0)
			{
				foreach (GameObject go in Pelo_Mujer)
				{
					go.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
				}
			}
					
			if(LColorPelo==1)
			{
				foreach (GameObject go in Pelo_Mujer)
				{
					go.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
				}
			}
					
			if(LColorPelo==2)
			{
				foreach (GameObject go in Pelo_Mujer)
				{
					go.GetComponent<SkinnedMeshRenderer>().material.color = Color.green;
				}
			}
					
			if(LColorPelo==3)
			{
				foreach (GameObject go in Pelo_Mujer)
				{
					go.GetComponent<SkinnedMeshRenderer>().material.color = Color.yellow;
				}
			}

			break;
					
					
		case "TipoPelo":
			//sumo y confirmo que no me pasare del array
			LPelo++;
			if(LPelo >= Pelo_Mujer.Length-1)
			{
				LPelo=0;	
			}
			foreach (GameObject item in Pelo_Mujer)
			{
				item.SetActive(false);
			}
			Pelo_Mujer[LPelo].SetActive(true);
			break;
					
		case "Camisa":
			//sumo y confirmo que no me pasare del array
			LCamisa++;
			if(LCamisa >= Camisa_Mujer.Length-1)
			{
				LCamisa=0;	
			}
			//cambio piel en cabezas
			H_PielMat[1] = Camisa_Mujer[LCamisa];
			pielBrazoMujer.materials = H_PielMat; 
			break;
					
		case "Sueter":
			//sumo y confirmo que no me pasare del array
			LChaqueta++;
			if(LChaqueta >= Color_Sueter_Mujer.Length-1)
			{
				LChaqueta=0;	
			}
			if(LChaqueta==0){
				Sueter_Mujer.SetActive(false);
			}
			else
			{
				Sueter_Mujer.SetActive(true);
				H_SueterMat = SueterMujer.materials;
				H_SueterMat[0] = Color_Sueter_Mujer[LChaqueta];
				SueterMujer.materials = H_SueterMat;	
			}
			break;
					
		case "Pantalones":
			//sumo y confirmo que no me pasare del array
			LPantalones++;
			if(LPantalones >= Camisa_Mujer.Length-1)
			{
				LPantalones=0;	
			}
			//cambio piel en cabezas
			H_PantsMat[0] = Color_Pantalones_Mujer[LPantalones];
			PantalonesMujer.materials = H_PantsMat; 
			break;
					
		case "Zapatos":
			//sumo y confirmo que no me pasare del array
			LZapato++;
			if(LZapato >= Color_Zapatos_Mujer.Length-1)
			{
				LZapato=0;	
			}
			//cambio piel en cabezas
			H_Zapatos[0] = Color_Zapatos_Mujer[LZapato];
			ZapatosMujer.materials = H_Zapatos; 
			break;
		}
		
	}
	
	
	void SaveData()
	{
		CustomPlayer["Genero"] = Lgenero;
		CustomPlayer["Piel"] = LPiel;
		CustomPlayer["Ojos"] = LOjos;
		CustomPlayer["ColorPelo"] = LColorPelo;
		CustomPlayer["TipoPelo"] = LPelo;
		CustomPlayer["Camisa"] = LCamisa;
		CustomPlayer["Chaqueta"] = LChaqueta;
		CustomPlayer["Pantalones"] = LPantalones;
		CustomPlayer["Zapatos"] = LZapato;
		PhotonNetwork.LocalPlayer.CustomProperties = CustomPlayer;
		
	}
	
	void  ChangeScene()
	{
		SaveData();
		mgr = FindObjectOfType<CargandoManager>();
		mgr.LogosCambioScena();
		fin();
			
	}
	
	void fin()
	{	
		init.CreateAndJoinRoom();
	}
	
	public override void OnJoinedRoom()
	{
		init.cambioScena(3);

	}
}
