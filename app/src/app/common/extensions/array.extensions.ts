declare global {
    interface Array<T> {
        distinct(predicate: (value: T) => any): T[];
        groupBy<T, K>(getKey: (item: T) => K): any;
    }
}

Array.prototype.distinct = function<T> (predicate: (value: T) => any): T[] {
    return [...new Map(this.map(item => [predicate(item), item])).values()];
};

Array.prototype.groupBy = function<T, K>(getKey: (item: T) => K): any {
    const map = new Map<K, T[]>();

    this.forEach((item) => {
        const key = getKey(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    
    return Array.from(map.values());
}

export { };