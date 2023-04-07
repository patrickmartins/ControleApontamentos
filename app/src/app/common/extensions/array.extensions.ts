declare global {
    interface Array<T> {
        distinct(predicate: (value: T) => any): T[];
    }
}

Array.prototype.distinct = function<T> (predicate: (value: T) => any): T[] {
    return [...new Map(this.map(item => [predicate(item), item])).values()];
};

export { };