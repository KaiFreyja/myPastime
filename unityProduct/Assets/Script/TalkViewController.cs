using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class TalkViewController : MonoBehaviour
{
    public Button btnSendTalk = null;
    public Button btnClose = null;
    public RawImage roleImage = null;
    public Text labName = null;
    public Text labDescription = null;
    public Text labAns = null;
    public InputField editText = null;

    private string tgid = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        btnSendTalk.onClick.AddListener(new UnityEngine.Events.UnityAction(sendTalk));
        btnClose.onClick.AddListener(new UnityEngine.Events.UnityAction(close));
    }

    public void open(JObject jobj)
    {
        labAns.text = string.Empty;
        editText.text = string.Empty;
        roleImage.texture = null;
        labName.text = string.Empty;
        labDescription.text = string.Empty;
        tgid = string.Empty;

        this.gameObject.SetActive(true);
        LoadRole(jobj);
        init(jobj);
    }

    private void LoadRole(JObject jobj)
    {
        var download = roleImage.GetComponent<ImageDownloader>();
        if (download == null)
        {
            download = roleImage.gameObject.AddComponent<ImageDownloader>();
        }
        download.StartDownload(Config.IMAGE_DOMAIN + jobj["rid"] + ".png", (Texture texture) =>
        {
            roleImage.texture = texture;
        });

        labName.text = jobj["name"].ToString();
        labDescription.text = jobj["description"].ToString();
    }

    private void init(JObject jobj)
    {
        string name = jobj["description"].ToString();
        string text = "重現在開始你是FGO的"+ name + "，請作為" + name + "跟我對話";
        JObject input = new JObject();
        input.Add("uid", GlobalData.uid);
        input.Add("ask", text);
        Debug.Log("input : " + input);
        APIController controller = new APIController();
        controller.AskAiTalk(input,(APIResult result)=>
        {
            JObject json = result.getData();
            Debug.Log("init : " + json);

            JObject talk_group_ask = json["talk_group_ask"] as JObject;
            string tgid = talk_group_ask["tgid"].ToString();
            string content = talk_group_ask["content"].ToString();
            this.tgid = tgid;
            labAns.text = content;
            var csf = labAns.GetComponent<ContentSizeFitter>();
            csf.enabled = false;
            csf.enabled = true;
            var pcsp = labAns.transform.parent.GetComponent<ContentSizeFitter>();
            pcsp.enabled = false;
            pcsp.enabled = true;
            csf.enabled = !csf.enabled;
        });
    }

    private void sendTalk()
    {
        string text = editText.text;
        Debug.Log("sendTalk : " + text);
        editText.text = string.Empty;


        JObject input = new JObject();
        input.Add("uid", GlobalData.uid);
        input.Add("tgid", tgid);
        input.Add("ask", text);
        Debug.Log("input : " + input);
        APIController controller = new APIController();
        controller.AskAiTalk(input, (APIResult result) =>
        {
            JObject json = result.getData();
            Debug.Log("init : " + json);

            JObject talk_group_ask = json["talk_group_ask"] as JObject;
            string content = talk_group_ask["content"].ToString();
            labAns.text = content;

        });

    }

    private void close()
    {
        tgid = string.Empty;
        this.gameObject.SetActive(false);
    }
}
