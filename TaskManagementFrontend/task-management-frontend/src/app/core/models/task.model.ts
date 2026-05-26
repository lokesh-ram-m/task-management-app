export enum TaskStatus {
  New        = 0,
  InProgress = 1,
  Done       = 2
}

export interface TaskResponse {
  id:                 number;
  title:              string;
  description:        string;
  status:             TaskStatus;
  assignedToId:       number;
  assignedToUsername: string;
  reporterId:         number;
  reporterUsername:   string;
  projectId:          number;
  projectName:        string;
  createdBy:          number;
  createdAt:          string;
  updatedAt:          string;
}

export interface CreateTaskRequest {
  title:        string;
  description:  string;
  assignedToId: number;
  reporterId:   number;
  projectId:    number;
}

export interface UpdateTaskRequest {
  title:        string;
  description:  string;
  status:       TaskStatus;
  assignedToId: number;
  reporterId:   number;
  projectId:    number;
}

export interface TaskFilter {
  assignedToIds?: number[];
  statuses?:      TaskStatus[];
  projectId?:     number;
}
