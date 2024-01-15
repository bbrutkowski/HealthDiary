import { AddressDto } from "./address-dto";
import { Gender } from "./enum/gender";

export class UserDto {
    public id: number;
    public name: string;
    public surname: string;
    public email: string;
    public age: number;
    public phoneNumber: string;
    public gender: Gender;
    public address: AddressDto;
    public birthDate: Date;
}