export class PostTerminationNotice {
    valueId?:number;


}

export class PostTerminationNoticeUploadDTO{
    file:Blob | null;
    notice_Duration:number;
    end_Date:Date;
    Remark:string;

    /**
     *
     */
    constructor(file:File | null,notice_duration:number,end_Date:Date,Remark:string) {
       this.file=file;
       this.notice_Duration=notice_duration;
       this.end_Date=end_Date;
       this.Remark=Remark;
        
    }
}
