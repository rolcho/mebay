import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  private storage: Storage;
  constructor() {
    this.storage = window.localStorage;
  }

  set(key: string, value: any): void {
    this.storage.setItem(key, value);
  }

  get<T>(key: string): T | null {
    const item = this.storage.getItem(key);
    try {
      if (item === null) return null;
      return JSON.parse(item);
    } catch {}
    return item as T;
  }

  remove(key: string): void {
    this.storage.removeItem(key);
  }

  clear(): void {
    this.storage.clear();
  }
}
