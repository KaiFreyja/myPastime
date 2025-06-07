import { _decorator, Component, Node, Sprite,Texture2D,SpriteFrame } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('RoleContentItemController')
export class RoleContentItemController extends Component {

    @property(Sprite)
    public img : Sprite;
    /*
    start() {

    }

    update(deltaTime: number) {
        
    }*/

    public setTexture(texture : Texture2D)
    {
        const spriteFrame = new SpriteFrame();
        spriteFrame.texture = texture;
        this.img.spriteFrame = spriteFrame;
    }
}


