import React from "react";
import { render, fireEvent, screen } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import axios from "axios";
import { AuthContext } from "../common/AuthContext";
import Login from "./Login";

jest.mock("axios");
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => jest.fn(),
}));

describe("Login Component", () => {
  const setAccount = jest.fn();
  const account = null;

  const renderComponent = () =>
    render(
      <AuthContext.Provider value={{ account, setAccount }}>
        <Router>
          <Login />
        </Router>
      </AuthContext.Provider>
    );

  beforeEach(() => {
    jest.clearAllMocks();
  });

  test("renders email and password input fields", () => {
    renderComponent();
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
  });

  test("displays error message on failed login", async () => {
    axios.post.mockRejectedValueOnce(new Error("Invalid email or password"));

    renderComponent();

    fireEvent.change(screen.getByLabelText(/email/i), {
      target: { value: "test@example.com" },
    });
    fireEvent.change(screen.getByLabelText(/password/i), {
      target: { value: "password" },
    });

    fireEvent.click(screen.getByText(/login/i));

    const errorMessage = await screen.findByText(
      /invalid email or password. please try again./i
    );
    expect(errorMessage).toBeInTheDocument();
  });

  test("navigates to dashboard on successful login", async () => {
    const mockNavigate = jest.fn();
    jest
      .spyOn(require("react-router-dom"), "useNavigate")
      .mockImplementation(() => mockNavigate);

    axios.post.mockResolvedValueOnce({ data: { user: "testUser" } });

    renderComponent();

    fireEvent.change(screen.getByLabelText(/email/i), {
      target: { value: "test@example.com" },
    });
    fireEvent.change(screen.getByLabelText(/password/i), {
      target: { value: "password" },
    });

    fireEvent.click(screen.getByText(/login/i));

    await screen.findByText(/login/i); // wait for the login button to reappear

    expect(setAccount).toHaveBeenCalledWith({ user: "testUser" });
    expect(mockNavigate).toHaveBeenCalledWith("/dashboard");
  });

  test("clears error message on input change", async () => {
    axios.post.mockRejectedValueOnce(new Error("Invalid email or password"));

    renderComponent();

    fireEvent.change(screen.getByLabelText(/email/i), {
      target: { value: "test@example.com" },
    });
    fireEvent.change(screen.getByLabelText(/password/i), {
      target: { value: "password" },
    });

    fireEvent.click(screen.getByText(/login/i));

    const errorMessage = await screen.findByText(
      /invalid email or password. please try again./i
    );
    expect(errorMessage).toBeInTheDocument();

    fireEvent.change(screen.getByLabelText(/email/i), {
      target: { value: "new@example.com" },
    });

    expect(errorMessage).not.toBeInTheDocument();
  });
});
