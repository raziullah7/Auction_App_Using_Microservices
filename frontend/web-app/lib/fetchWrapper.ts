import { auth } from "@/auth";

const baseUrl = "http://localhost:6001/";

// GET request from the backend
async function get(url: string) {
  const requestOptions = {
    method: "GET",
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return handleResponse(response);
}

// POST request from the backend
async function post(url: string, body: {}) {
  const requestOptions = {
    method: "POST",
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return handleResponse(response);
}

// PUT request from the backend
async function put(url: string, body: {}) {
  const requestOptions = {
    method: "PUT",
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return handleResponse(response);
}

// DELETE request from the backend
async function del(url: string) {
  const requestOptions = {
    method: "DELETE",
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return handleResponse(response);
}

// function to get headers for each request from the backend
async function getHeaders() {
  const session = await auth();
  const headers = {
    "Content-type": "application/json",
  } as any;

  if (session?.accessToken) {
    headers.Authorization = "Bearer " + session.accessToken;
  }

  return headers;
}

// method to handle the response received from the backend
async function handleResponse(response: Response) {
  const text = await response.text();
  const data = text && JSON.parse(text);

  if (response.ok) {
    return data || response.statusText;
  } else {
    const error = {
      status: response.status,
      message: response.statusText,
    };
    return { error };
  }
}

// separating the export instead of making the function
// itself exported for abstraction
export const fetchWrapper = {
  get,
  post,
  put,
  del,
};
