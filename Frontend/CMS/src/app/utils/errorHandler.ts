import { TYPE } from "../components/auth/login/values.constants";
import { Alert } from "./alert";

export class ErrorHandler {
    static handle(error: any) {
        let errorMessage;
        console.error('Error :(', error);
        if(error.status == 0){
            
            errorMessage = JSON.stringify("Unable to connect with the Server.");
        }
        else if (error.status == 401) {
            errorMessage = JSON.stringify(error.error);
        }
         else if (error.error !== undefined && error.error.message !== undefined) {
            errorMessage = error.error.message;
        }
        else if (error.title !== undefined) {
            errorMessage = JSON.stringify(error.title);
        }
        else if (error.error !== undefined && error.error.title !== undefined) {
            errorMessage = JSON.stringify(error.error.title);
        }
        else if (error.message !== undefined) {
            errorMessage = error.message;
        }
        else if (error.error !== undefined && error.error.message !== undefined) {
            errorMessage = JSON.stringify(error.error.message);
        }
        else {
            errorMessage = "Something went wrong."
        }
        Alert.toast(TYPE.ERROR, true, errorMessage);
    }
}