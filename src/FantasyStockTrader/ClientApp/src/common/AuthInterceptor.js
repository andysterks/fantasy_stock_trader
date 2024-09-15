import axios from "axios";
import globalRouter from "./GlobalRouter";

const instance = axios.create();

instance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        await axios.post("/api/auth/refresh-token");
        return instance(originalRequest);
      } catch (err) {
        console.error("Failed to refresh token:", err);
        // Redirect to login page
        if (globalRouter.navigate) {
          globalRouter.navigate("/login");
        }
        return Promise.reject(err);
      }
    }
    return Promise.reject(error);
  }
);

export default instance;
