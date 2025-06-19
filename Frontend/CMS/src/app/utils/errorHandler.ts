import { TYPE } from "../components/auth/login/values.constants";
import { Alert } from "./alert";

export class ErrorHandler {
    static handle(error: any) {
        let errorMessage;
        if (error.status == 401) {
            errorMessage = error.error;
        }
         else if (error.error !== undefined && error.error.message !== undefined) {
            errorMessage = error.error.message;
        }
        else if (error.title !== undefined) {
            errorMessage = error.title;
        }
        else if (error.error !== undefined && error.error.title !== undefined) {
            errorMessage = error.error.title
        }
        else if (error.message !== undefined) {
            errorMessage = error.message;
        }
        else {
            errorMessage = "Something went wrong."
        }
        Alert.toast(TYPE.ERROR, true, errorMessage);
    }
}