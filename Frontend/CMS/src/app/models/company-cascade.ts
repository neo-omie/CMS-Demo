export class Countriess {
    countryId:number;
    countries:string;
    constructor(CountryId:number, Countries:string){
        this.countryId = CountryId;
        this.countries = Countries;
    }
}

export class States {
    countryId:number;
    stateId:number;
    state:string;
    constructor(CountryId:number, StateId:number, State:string){
        this.countryId = CountryId;
        this.state = State;
        this.stateId = StateId;
    }
}


export class Cities {
    stateId:number;
    cityId:number;
    city:string;
    constructor(StateId:number, CityId:number, City:string){
        this.stateId = StateId;
        this.cityId = CityId;
        this.city = City;
    }
}