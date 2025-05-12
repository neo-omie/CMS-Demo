import { TemplateRef } from "@angular/core"

export class ColumnTemplates {
    title:string
    templateRef:{[key:string] : TemplateRef<any>}
    constructor(title:string, templateRef:{[key:string] : TemplateRef<any>}){
        this.title = title;
        this.templateRef = templateRef;
    }
}
