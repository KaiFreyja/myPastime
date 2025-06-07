import { _decorator, Component, Node } from 'cc';
import {ViewController} from './ViewController'

const { ccclass, property } = _decorator;

@ccclass('example2ViewController')
export class example2ViewController extends ViewController {
    
    protected init(): void {
        super.init();
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

    public unitTest() :void
    {
        console.log("unitTest");
    }
}


