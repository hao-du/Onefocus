class TokenManager {
  private static instance: TokenManager;
  private accessToken: string | null = null;
  private expiresAt: number = 0; // stored as UTC timestamp (ms)
  private refreshPromise: Promise<string> | null = null;

  private constructor() {}

  public static getInstance() {
    if (!TokenManager.instance) TokenManager.instance = new TokenManager();
    return TokenManager.instance;
  }

  public setToken(token: string, expiresAtUtc: string | Date) {
    this.accessToken = token;
    const date = expiresAtUtc instanceof Date ? expiresAtUtc : new Date(expiresAtUtc);
    this.expiresAt = date.getTime();
  }

  public getToken() {
    return this.accessToken;
  }

  public clear() {
    this.accessToken = null;
    this.expiresAt = 0;
  }

  public isTokenExpiringSoon(minutes = 2) {
    if (!this.expiresAt) return false;
    return this.expiresAt - Date.now() < minutes * 60 * 1000;
  }

  public isTokenValid() {
    return this.accessToken && Date.now() < this.expiresAt;
  }

  public setRefreshPromise(promise: Promise<string> | null) {
    this.refreshPromise = promise;
  }

  public getRefreshPromise() {
    return this.refreshPromise;
  }
}

export const tokenManager = TokenManager.getInstance();