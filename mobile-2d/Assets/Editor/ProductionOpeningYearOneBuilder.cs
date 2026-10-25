using UnityEditor;
using UnityEngine;

public static partial class ProductionOpeningSceneBuilder
{
    private static void BuildYearOneWorld(ProductionOpeningController c)
    {
        c.queueStudents=new GameObject[3];
        for(int i=0;i<3;i++)
        {
            c.queueStudents[i]=Render(c.zones[2],"QueueStudent"+i,Art("C00","npc_freshman_"+(i+1)),new Vector2(-.8f-i*.8f,-.75f),.45f,18).gameObject;
            c.queueStudents[i].SetActive(false);
        }
        var room=c.zones[3];
        c.storedCooker=Item(room,"StoredCooker",Legacy("year1/pot_0"),new Vector2(-2.5f,-2.4f),.6f,15);
        c.feastPot=Item(room,"FeastPot",Legacy("year1/pot_0"),new Vector2(0,-2.45f),.9f,22);
        c.feastPotFrames=new[]{Legacy("year1/pot_0"),Legacy("year1/pot_1")};
        Item(room,"CookerSocket",Legacy("environment/light_switch_socket"),new Vector2(2.4f,.3f),.35f,5);
        var cord=Node("CookerCord",room,Vector2.zero);c.feastCord=cord;
        var line=cord.AddComponent<LineRenderer>();line.useWorldSpace=false;line.positionCount=4;line.SetPosition(0,new Vector3(2.4f,.3f,0));line.SetPosition(1,new Vector3(2.7f,-1.3f,0));line.SetPosition(2,new Vector3(2.7f,-2.2f,0));line.SetPosition(3,new Vector3(0,-2.45f,0));line.startWidth=line.endWidth=.035f;line.startColor=line.endColor=new Color(.07f,.08f,.09f);line.sortingOrder=8;
        const string materialPath="Assets/AssetProduction/HotpotCord.mat";
        var material=AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if(material==null){material=new Material(Shader.Find("Sprites/Default"));AssetDatabase.CreateAsset(material,materialPath);}line.sharedMaterial=material;
        var food=Node("FeastIngredients",room,Vector2.zero);c.feastIngredients=food;
        Item(food.transform,"DriedBuffaloMeat",Art("P04","dried_buffalo_meat"),new Vector2(-.65f,-2.35f),.2f,23);
        Item(food.transform,"Vegetables",Art("P00","food_vegetables"),new Vector2(.65f,-2.35f),.2f,23);
        Item(food.transform,"SharedDrink",Art("P05","drinking_glass"),new Vector2(2.1f,-2.2f),.3f,23);
        c.feastActors=new GameObject[4];string[] names={"luon","dam","long","to"};
        Vector2[] positions={new Vector2(-1.2f,-2.85f),new Vector2(1.2f,-2.85f),new Vector2(-1.2f,-1.75f),new Vector2(1.2f,-1.75f)};
        for(int i=0;i<4;i++){c.feastActors[i]=Render(room,"Feast_"+names[i],Legacy("year1/"+names[i]+"_floor"),positions[i],.58f,i<2?25:18).gameObject;c.feastActors[i].SetActive(false);}
        c.storedCooker.enabled=false;c.feastPot.enabled=false;cord.SetActive(false);food.SetActive(false);
    }
}
