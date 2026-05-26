import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateTaskRequest, TaskFilter, TaskResponse, UpdateTaskRequest } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly apiUrl = 'http://localhost:5114/api/tasks';

  constructor(private http: HttpClient) {}

  getAll(filter?: TaskFilter): Observable<TaskResponse[]> {
    let params = new HttpParams();
    if (filter?.projectId)     params = params.set('projectId', filter.projectId);
    if (filter?.assignedToIds) params = params.set('assignedToIds', filter.assignedToIds.join(','));
    if (filter?.statuses)      params = params.set('statuses', filter.statuses.join(','));
    return this.http.get<TaskResponse[]>(this.apiUrl, { params });
  }

  getById(id: number): Observable<TaskResponse> {
    return this.http.get<TaskResponse>(`${this.apiUrl}/${id}`);
  }

  add(request: CreateTaskRequest): Observable<void> {
    return this.http.post<void>(this.apiUrl, request);
  }

  update(id: number, request: UpdateTaskRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
