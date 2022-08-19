export class Role{
    constructor(name?: string){
        if (name){
            this.name = name;
            if(!this.key){
                this.key = name;
            }
        }
    }

    id?: number;
    name: string | undefined;
    key: string | undefined;
}
