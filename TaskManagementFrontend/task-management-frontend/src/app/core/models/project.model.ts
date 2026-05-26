export interface ProjectResponse {
  id:                number;
  name:              string;
  description:       string;
  createdBy:         number;
  createdByUsername: string;
  createdAt:         string;
  updatedAt:         string;
}

export interface CreateProjectRequest {
  name:        string;
  description: string;
}
