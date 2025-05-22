using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.EventSystems;

public class RoleListViewController : ViewController
{

    public RoleListItemController item = null;
    public GameObject scrollContent = null;

    protected override void init()
    {
        base.init();
        item.gameObject.SetActive(false);
        onCreateRoleList();
    }

    protected override void open(object obj)
    {
        base.open(obj);
        onCreateRoleList();
    }

    public void onCreateRoleList()
    {
        Debug.Log("onCreateRoleList");

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
                var co = g.GetComponent<RoleListItemController>();
                co.SetName(a["name"].ToString());
                co.onClick += () =>
                {
                    ViewController.GetViewController(typeof(RoleContentViewController)).show(a as JObject);
                    //FindObjectOfType<RoleContentViewController>().open(a as JObject);
                };
                g.SetActive(true);
                var download = g.AddComponent<ImageDownloader>();
                download.StartDownload(a["url"].ToString(),(Texture texture)=>
                {
                    co.SetImage(texture);
                });
                if (count == 0)
                {
                    co.onClick.Invoke();
                }
                count++;
            }
        });
    }
}
