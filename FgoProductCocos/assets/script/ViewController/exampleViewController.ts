import { _decorator, Button, Component, Node } from 'cc';
import {ViewController} from './ViewController'
import { example2ViewController } from './example2ViewController';
const { ccclass, property } = _decorator;

@ccclass('exampleViewController')
export class exampleViewController extends ViewController {
    
    @property(Button)
    public btn : Button;

    protected init(): void {
        super.init();
        this.btn.node.on(Button.EventType.CLICK,this.send,this);
    }

    protected open(obj: any): void {
        super.open(obj);
    }

    public close(): void {
        super.close();
    }

    protected onTimer(): void {
        super.onTimer();
    }

    async send()
    {
        var view = await ViewController.GetViewController(example2ViewController);
        view.show();
        view.unitTest();
    }
}


