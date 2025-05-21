import { AddAddendumContract } from "./add-addendum-contract";

export class AddendumContract {
    data:AddAddendumContract[];
    totalCount:number;
    constructor(data:AddAddendumContract[], totalCount:number){
        this.data=data;
        this.totalCount=totalCount;
    }
}
