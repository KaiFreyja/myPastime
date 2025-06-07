import { _decorator, Component, error, find, Node, Prefab, resources,instantiate } from 'cc';
import { Config } from '../Config';
import { ViewControllerManager } from './ViewControllerManager';
const { ccclass, property } = _decorator;

@ccclass('ViewController')
export class ViewController extends Component {

    private static canvas : Node;
    private static viewControllerTemps : Map<Function,ViewController> = new Map();
    
    public static async GetViewController<T extends ViewController>(vcClass: { new(): T }): T
    {
        console.log("vcClass : " + vcClass.name);

        if(!this.viewControllerTemps.has(vcClass))
        {
            if(this.canvas == null)
            {
                this.canvas = find("Canvas");
            }
            if(Config.IS_ASSEST_BUNDLE)
            {

            }
            else
            {
                const path = "ui/" + (vcClass as any).viewResource();
                const node = await this.loadAndInstantiatePrefab(path);
                //const node = await this.loadAndInstantiatePrefab("ui/" + vcClass.name);
            }
        }

        return this.viewControllerTemps.get(vcClass) as T;
    }

    private static async loadAndInstantiatePrefab(path: string): Promise<Node> {
        return new Promise((resolve, reject) => {
            resources.load(path, Prefab, (err, prefab) => {
                if (err || !prefab) {
                    reject(err);
                } else {
                    const node = instantiate(prefab);
                    this.canvas.addChild(node);
                    resolve(node);
                }
            });
        });
    }

    protected static viewResource():string
    {
        return "ViewController";
    }

    private isTryOpen : boolean = false;
    private openData : any = null;
    protected onLoad(): void {
        //console.log(this.constructor.name);
        var ct = this.constructor as Function;
        //console.log("ct : " + ct);
        if(!ViewController.viewControllerTemps.has(ct))
        {
            ViewController.viewControllerTemps.set(ct,this);
        }
    }

    start() {
        this.init();
    }

    update(deltaTime: number) {
        if(this.isTryOpen)
        {
            this.isTryOpen = false;
            var input = this.openData;
            this.openData = null;
            this.open(input);
        }

        this.onTimer();
    }   

    protected init():void
    {

    }

    protected open(obj : any):void
    {

    }

    public close() : void
    {
        this.node.active = false;
    }

    protected onTimer()
    {

    }

    
    public show():void;
    public show(data : any):void;
    public show(data? : any):void
    {
        this.node.active = true;
        this.isTryOpen = true;
        if(data)
        {
            this.openData = data;
        }
    }
}


