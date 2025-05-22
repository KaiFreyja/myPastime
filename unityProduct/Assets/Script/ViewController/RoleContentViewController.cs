using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.U2D;

public class RoleContentViewController : ViewController
{
    public RoleContentImageItemController item = null;
    public GameObject ImageContent = null;
    public Text LabelName = null;
    public Text LabelDescription = null;
    public Image iconProfession = null;
    public Button btnTalk = null;
    private JObject info = new JObject();
    //public TalkViewController talkViewController = null;

    protected override void init()
    {
        base.init();
        btnTalk.onClick.AddListener(new UnityEngine.Events.UnityAction(openTalk));
        item.gameObject.SetActive(false);
    }

    protected override void open(object obj)
    {
        open(obj as JObject);
    }

    private void openTalk()
    {
        ViewController.GetViewController(typeof(TalkViewController)).show(info);
        //talkViewController.open(info);
    }

    public void open(JObject jobj)
    {
        Debug.Log("open : " + jobj);

        info = jobj;
        showData(jobj);
        clearImageViews();
        createImageViews(jobj);
    }

    private void showData(JObject jobj)
    {
        LabelName.text = jobj["name"].ToString();
        LabelDescription.text = jobj["description"].ToString();

        Sprite sprite = Resources.Load<Sprite>("icon_profession/"+ jobj["pid"].ToString());
        iconProfession.sprite = sprite;
    }

    private void clearImageViews()
    {
        var childCount = ImageContent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var g = ImageContent.transform.GetChild(i);
            if (g.gameObject.activeSelf)
            {
                GameObject.Destroy(g.gameObject);
            }
        }
    }

    private void createImageViews(JObject jobj)
    {
        string rid = jobj["rid"].ToString();
        APIController controller = new APIController();
        JObject input = new JObject();
        input.Add("rid", rid);
        controller.GetFgoRoleResource(input, (APIResult result) =>
        {
            JObject json = result.getData();
            JArray data = json["role_resource"]["data"] as JArray;

            int count = 0;
            foreach (var a in data)
            {
                count++;
                var g = GameObject.Instantiate(item.gameObject);
                g.name = count + "";
                var co = g.GetComponent<RoleContentImageItemController>();
                g.transform.SetParent(ImageContent.transform);
                g.transform.localScale = Vector3.one;
                g.SetActive(true);
                string url = a["url"].ToString();
                var download = g.AddComponent<ImageDownloader>();
                download.StartDownload(url, (Texture t) =>
                {
                    co.setTexture(t);
                });
            }
        });
    }
}
