import { _decorator, Component, Node,Sprite,Label,SpriteFrame,Texture2D, Button } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('RoleListItemController')
export class RoleListItemController extends Component {

    @property(Button)
    public btn : Button;
    @property(Sprite)
    public img : Sprite;
    @property(Label)
    public lab : Label;

    public onClick : ()=>void;
    
    start() {
        this.btn.node.on(Button.EventType.CLICK,this.onClick,this);
    }

    /*
    update(deltaTime: number) {
        
    }*/

    public setText(text : string)
    {
        this.lab.string = text;
    }

    public setTexture(texture : Texture2D)
    {
        const spriteFrame = new SpriteFrame();
        spriteFrame.texture = texture;
        this.img.spriteFrame = spriteFrame;
    }
}


