import Airplane from "./airplane.model";

class Station {
    id: number = 0;
    planeName?: string;
    plane?: Airplane;
    state: boolean = false;
}

export default Station;