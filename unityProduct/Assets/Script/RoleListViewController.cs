using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class RoleListViewController : MonoBehaviour
{

    public RoleListItemController item = null;
    public GameObject scrollContent = null;

    // Start is called before the first frame update
    void Start()
    {
        item.gameObject.SetActive(false);
        onCreateRoleList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCreateRoleList()
    {
        APIController controller = new APIController();
        controller.GetFgoGetRole(new JObject(),(APIResult result) =>
        {
            JObject json = result.getData();

            Debug.Log(json);

            JArray data = json["role"]["data"] as JArray;
            int count = 0;
            foreach (var a in data)
            {
                var g = GameObject.Instantiate(item.gameObject);
                g.transform.SetParent(scrollContent.transform);
                g.transform.localScale = Vector3.one;
                g.name = ""+count;
                count++;
                var co = g.GetComponent<RoleListItemController>();
                co.SetName(a["name"].ToString());
                g.SetActive(true);
                var download = g.AddComponent<ImageDownloader>();
                download.StartDownload(Config.IMAGE_DOMAIN + a["rid"] + ".png",(Texture texture)=>
                {
                    co.SetImage(texture);
                });
            }
        });
    }
}
