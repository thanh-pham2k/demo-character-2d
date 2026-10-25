using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DormJourneySceneSetup;

public static partial class ProductionOpeningSceneBuilder
{
    private static void BuildRegistrationWorld(ProductionOpeningController c)
    {
        c.registrationLaptop=Item(c.zones[3],"RegistrationLaptop",Legacy("props/laptop_open"),new Vector2(-.48f,.55f),.5f,16);
        c.registrationTyping=Render(c.zones[3],"RegistrationTyping",Legacy("commute/typing_0"),new Vector2(-1,0),.65f,15);c.registrationTyping.enabled=false;
        c.registrationTypingFrames=new[]{Legacy("commute/typing_0"),Legacy("commute/typing_1")};
    }
    private static void BuildRegistrationUI(ProductionOpeningController c,RectTransform modal)
    {
        var panel=Panel(modal,"RegistrationPortal",new Vector2(.5f,.5f),new Vector2(0,35),new Vector2(950,330),Color.clear);c.registrationPanel=panel.gameObject;
        c.registrationStatus=LabelAt(panel,"Status","08:59",new Vector2(0,100),new Vector2(900,44),21);
        c.registrationClasses=new Button[3];
        for(int i=0;i<3;i++)
        {
            var button=Button(panel,"Course"+i,"",new Vector2(0,55-i*45));button.GetComponent<RectTransform>().sizeDelta=new Vector2(900,38);button.GetComponentInChildren<TMP_Text>().fontSize=18;c.registrationClasses[i]=button;
        }
        c.registrationFullClass=Button(panel,"Full0900","GT2-0900 • 09:00 • Hết chỗ",new Vector2(0,-78));c.registrationFullClass.GetComponent<RectTransform>().sizeDelta=new Vector2(650,30);c.registrationFullClass.GetComponentInChildren<TMP_Text>().fontSize=17;c.registrationFullClass.interactable=false;
        c.registrationSummary=LabelAt(panel,"Summary","",new Vector2(0,40),new Vector2(900,90),22);c.registrationSummary.textWrappingMode=TextWrappingModes.Normal;
        c.registrationRefresh=Button(panel,"Refresh","[F5] Tải lại cổng đăng ký",new Vector2(0,-70));c.registrationRefresh.GetComponent<RectTransform>().sizeDelta=new Vector2(650,48);
        panel.gameObject.SetActive(false);
    }
}
