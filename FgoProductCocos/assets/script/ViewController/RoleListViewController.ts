import { _decorator, assetManager, Component, error, instantiate, Node, Texture2D,ImageAsset, find  } from 'cc';
import { APIController,APIResult } from '../APIController';
import { GlobalData } from '../GlobalData';
import { RoleListItemController } from './RoleListItemController';
import { RoleContentViewController } from './RoleContentViewController';
import { ViewController } from './ViewController';
const { ccclass, property } = _decorator;

@ccclass('RoleListViewController')
export class RoleListViewController extends ViewController {
    
    @property(RoleListItemController)
    public item: RoleListItemController;
    @property(Node)
    public content: Node;

    protected static viewResource(): string {
        return "RoleListViewController";
    }

    protected init(): void {
        this.item.node.active = false;   
    }

    protected open(obj: any): void {
        this.createView(); 
    }

    private createView()
    {
        let controller : APIController = new APIController();
        let input = {"uid":GlobalData.uid};
        controller.GetFgoGetRole(input,(result : APIResult) =>
        {
            var data = result.getData();
            var role = data["role"]["data"];
            console.log(role);
            let count = 0;
            role.forEach(unit => {
                
                var gobj = instantiate(this.item.node);
                gobj.setParent(this.content);
                gobj.active = true;
                var co = gobj.getComponent(RoleListItemController);
                co.onClick = () =>
                {
                    this.sendContent(unit);
                };

                co.setText(unit.name);
                var url = unit.url;
                assetManager.loadRemote<ImageAsset>(url, { ext: '.png' }, (err, imageAsset) => 
                {
                    if (err|| !imageAsset) {
                        console.error("載入圖片錯誤：", err);
                        return;
                    }
                    const texture = new Texture2D();
                    texture.image = imageAsset;
                    co.setTexture(texture);
                });
                

                if(count == 0)
                {
                    co.onClick();
                }
                count++;
            });
        });
    }

    private async sendContent(unit : any)
    {
        var view = await ViewController.GetViewController(RoleContentViewController);
        view.show(unit);
    }
}


