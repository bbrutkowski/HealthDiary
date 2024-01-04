export class OperationResult<T> {
    public data: T;
    public isSuccess: boolean;
    public isFailure: boolean;
    public errorMessage?: string;

    static Success<T>(data: T): OperationResult<T> {
        return {
            isSuccess: true,
            isFailure: false,
            data: data
        };
    }

    static Failure<T>(error: string): OperationResult<T> {
        return {
            isSuccess: false,
            isFailure: true,
            data: null as any,
            errorMessage: error
        };
    }
}