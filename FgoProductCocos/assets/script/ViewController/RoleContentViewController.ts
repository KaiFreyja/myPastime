import { _decorator, assetManager, Button, Component, error, ImageAsset, instantiate, Label, Node ,PageView,Prefab,resources,Sprite,SpriteFrame,Texture2D} from 'cc';
import { APIController, APIResult } from '../APIController';
import { GlobalData } from '../GlobalData';
import { RoleContentItemController } from './RoleContentItemController';
import { ViewController } from './ViewController';
import { TalkViewController } from './TalkViewController';
const { ccclass, property } = _decorator;

@ccclass('RoleContentViewController')
export class RoleContentViewController extends ViewController {

    @property(PageView)
    public pageView : PageView;
    @property(RoleContentItemController)
    public item : RoleContentItemController;
    @property(Node)
    public content : Node; 

    @property(Label)
    public labName : Label;
    @property(Label)
    public labDescription : Label;
    @property(Sprite)
    public iconProfession : Sprite;
    @property(Button)
    public btnTalk : Button;

    private pageCount : number = 0;
    private pageCurr : number = 0;

    protected static viewResource(): string {
        return "RoleContentViewController";
    }

    protected init(): void {
        this.item.node.active = false;
        this.schedule(this.autoScroll,3);       
    }

    protected open(obj: any): void {
        if(!obj)
        {
            return;
        }
        console.log(obj.description);
        this.showData(obj);
        this.clearImageView();
        this.createImageView(obj);
        this.btnTalk.node.on(Button.EventType.CLICK,async ()=>{
            var view = await ViewController.GetViewController(TalkViewController);
            view.show(obj);
        },this) 
    }

    private showData(data : any):void
    {
        this.labName.string = data.name;
        this.labDescription.string = data.description;
console.log("data.pid : " + data.pid);

        resources.load("icon_profession/" + data.pid + "/spriteFrame",SpriteFrame,(error,asset)=>
        {
            if(error)
            {
                console.log("error");
                return;
            }
            this.iconProfession.spriteFrame = asset;
        });
    }

    private clearImageView():void
    {
        this.pageCount = 0;
        this.pageCurr = 0;
        this.pageView.removeAllPages();
        var nodes = this.content.children;
        for(var i = 0; i < nodes.length; i++)
        {
            var node = nodes[i];
            if(node.active)
            {
                node.destroy();
            }
        }
    }


    private createImageView(data : any):void
    {
        let controller = new APIController();
        var input = {"uid":GlobalData.uid,"rid":data.rid};
        controller.GetFgoRoleResource(input,(result : APIResult)=>
        {
            var data = result.getData();
            console.log(data);
            var role_resource = data["role_resource"]["data"];
            this.pageCount = role_resource.length;
            this.pageCurr = 0;
            role_resource.forEach(unit =>{
                var gobj = instantiate(this.item.node);
                gobj.setParent(this.content);
                gobj.active = true;
                this.pageView.addPage(gobj)
                var co = gobj.getComponent(RoleContentItemController);
                var url = unit.url;
                assetManager.loadRemote<ImageAsset>(url,{exit:'png'},(err,imageAsset)=>
                {
                    if (err|| !imageAsset) {
                        console.error("載入圖片錯誤：", err);
                        return;
                    }
                    const texture = new Texture2D();
                    texture.image = imageAsset;
                    co.setTexture(texture);
                });
            });
        });
    }

    private autoScroll() : void
    {
        if(this.pageCount == 0)
            return;

        this.pageCurr = (this.pageCurr + 1) % this.pageCount;
        this.pageView.scrollToPage(this.pageCurr,0.5);
    }
}


