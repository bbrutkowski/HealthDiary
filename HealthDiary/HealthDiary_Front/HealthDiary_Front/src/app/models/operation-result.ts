export class Result<T> {
    public value: T;
    public isSuccess: boolean;
    public isFailure: boolean;
    public error: string;

    static Success<T>(value: T): Result<T> {
        return {
            isSuccess: true,
            isFailure: false,           
            value: value,
            error: null
        };
    }

    static Failure<T>(error: string): Result<T> {
        return {
            isSuccess: false,
            isFailure: true,
            value: null,
            error: error
        };
    }
}