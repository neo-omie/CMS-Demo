export class Login {
    email:string;
    password:string;
    constructor(email:string, password:string)
    {
        this.email = email;
        this.password = password;
    }
}
export class AuthResponse {
    id:string;
    email:string;
    name:string;
    token:string;
    constructor(id:string, email:string, name:string, token:string)
    {
        this.id = id;
        this.email = email;
        this.name = name;
        this.token = token;
    }
}