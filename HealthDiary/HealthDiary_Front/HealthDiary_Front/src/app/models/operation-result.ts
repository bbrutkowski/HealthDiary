export class Result<T> {
    public data: T;
    public isSuccess: boolean;
    public isFailure: boolean;
    public error: string;

    static Success<T>(data: T): Result<T> {
        return {
            isSuccess: true,
            isFailure: false,           
            data: data,
            error: null
        };
    }

    static Failure<T>(error: string): Result<T> {
        return {
            isSuccess: false,
            isFailure: true,
            data: null,
            error: error
        };
    }
}