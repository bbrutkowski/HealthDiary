export class Result<T> {
    public data: T;
    public isSuccess: boolean;
    public isFailure: boolean;
    public errorMessage: string;

    static Success<T>(data: T): Result<T> {
        return {
            isSuccess: true,
            isFailure: false,           
            data: data,
            errorMessage: null
        };
    }

    static Failure<T>(errorMessage: string): Result<T> {
        return {
            isSuccess: false,
            isFailure: true,
            data: null,
            errorMessage: errorMessage
        };
    }
}