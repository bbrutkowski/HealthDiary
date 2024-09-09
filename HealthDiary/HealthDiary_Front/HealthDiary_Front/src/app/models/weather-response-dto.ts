export interface WeatherResponseDto {
    name: string;
    main: MainDto;
    weather: WeatherDto[];
}
  
export interface MainDto {
    temp: number;
}
  
export interface WeatherDto {
    description: string;
    icon: string;
}