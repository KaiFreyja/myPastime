import { _decorator, assetManager, BatchedSkinningModelComponent, Button, Component, EditBox, ImageAsset, Label, Node, Sprite, SpriteFrame, Texture2D, url } from 'cc';
import { ViewController } from './ViewController';
import { GlobalData } from '../GlobalData';
import { APIController, APIResult } from '../APIController';
const { ccclass, property } = _decorator;

@ccclass('TalkViewController')
export class TalkViewController extends ViewController {
    
    @property(Sprite)
    public img : Sprite;
    @property(Label)
    public labName : Label;
    @property(Label)
    public labDescription : Label;
    @property(Label)
    public labAns : Label;
    @property(EditBox)
    public editTalk : EditBox;
    @property(Button)
    public btnTalk : Button;
    @property(Button)
    public btnClose : Button;

    private tgid : String;

    protected static viewResource(): string {
        return "TalkViewController";
    }

    protected init(): void {
        super.init();
        this.btnClose.node.on(Button.EventType.CLICK,this.close,this);
        this.btnTalk.node.on(Button.EventType.CLICK,this.sendTalk,this);
    }

    protected open(obj: any): void {
        super.open(obj);
        this.showRoleData(obj);
        this.initTalk(obj);
    }

    private showRoleData(obj : any) : void
    {
        this.labName.string = obj.name;
        this.labDescription.string = obj.description;
        this.img.spriteFrame = null;
        assetManager.loadRemote<ImageAsset>(obj.url,{ext: '.png'},(error,asset)=>
        {
            if(error || !asset)
            {
                return;
            }
            const texture = new Texture2D();
            texture.image = asset;
            const spriteFrame = new SpriteFrame();
            spriteFrame.texture = texture;
            this.img.spriteFrame = spriteFrame;
        });
    }

    public close(): void {
        super.close();
        this.labName.string = "";
        this.labDescription.string = "";
        this.img.spriteFrame = null;
    }

    private initTalk(obj : any) : void
    {
        const name : string = obj.description;
        const text : string = "從現在開始你是FGO的" + name + "，請作為" + name + "跟我對話";
        const input  = {"uid":GlobalData.uid,"ask":text};        
        let controller : APIController = new APIController();
        controller.AskAiTalk(input,(result : APIResult)=>
        {
            var data = result.getData();
            var talk_group_ask = data["talk_group_ask"];
            var tgid = talk_group_ask.tgid;
            var content = talk_group_ask.content;

            this.tgid = tgid;
            this.labAns.string = content;
        });
    }

    private sendTalk() : void
    {
        let text : string = this.editTalk.string;
        const input = {"uid":GlobalData.uid,"tgid":this.tgid,"ask":text};
        this.editTalk.string = "";
        let controller : APIController = new APIController();
        controller.AskAiTalk(input,(result : APIResult)=>
        {
            var data = result.getData();
            var talk_group_ask = data["talk_group_ask"];
            var tgid = talk_group_ask.tgid;
            var content = talk_group_ask.content;
            this.labAns.string = content;
        });
    }
}