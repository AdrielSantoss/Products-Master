export class JsonStorage<T> {
    constructor(private storage: Storage, private key: string) { }

    load() {
        const str = this.storage.getItem(this.key);
        if (str) {
            try {
                return JSON.parse(str) as T;
            } catch { }
        }
        return null;
    }

    save(obj: T) {
        this.storage.setItem(this.key, JSON.stringify(obj));
    }

    delete() {
        this.storage.removeItem(this.key);
    }
}

export class JsonLocalStorage<T> extends JsonStorage<T> {
    constructor(key: string) {
        super(localStorage, key);
    }
}

export class JsonSessionStorage<T> extends JsonStorage<T> {
    constructor(key: string) {
        super(sessionStorage, key);
    }
}
