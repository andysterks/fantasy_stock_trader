import axios from "axios";
import globalRouter from "./GlobalRouter";

axios.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const response = await axios.post(
          "/api/auth/refresh-token"
        );
        return axios(originalRequest);
      } catch (err) {
        console.error("Failed to refresh token:", err);
        // Handle the error, e.g., redirect to the login page
        console.error("TODO: REDIRECT TO LOGIN PAGE")
        if (globalRouter.navigate) {
            globalRouter.navigate("/")
        }
        return Promise.reject(err);
      }
    }
    return Promise.reject(error);
  }
);

export default axios;
