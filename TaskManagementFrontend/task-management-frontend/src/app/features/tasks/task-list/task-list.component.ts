import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgFor, NgIf, NgClass } from '@angular/common';
import { forkJoin } from 'rxjs';

import { TaskService } from '../../../core/services/task.service';
import { UserService } from '../../../core/services/user.service';
import { ProjectService } from '../../../core/services/project.service';
import { AuthService } from '../../../core/services/auth.service';

import {
  TaskResponse,
  TaskStatus,
  CreateTaskRequest,
  UpdateTaskRequest,
} from '../../../core/models/task.model';
import { UserResponse } from '../../../core/models/user.model';
import { ProjectResponse } from '../../../core/models/project.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, NgFor, NgIf, NgClass],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss',
})
export class TaskListComponent implements OnInit {
  tasks: TaskResponse[] = [];
  filteredTasks: TaskResponse[] = [];
  users: UserResponse[] = [];
  projects: ProjectResponse[] = [];

  showModal = false;
  isEditMode = false;
  selectedTaskId: number | null = null;
  filterStatus = '';

  taskForm!: FormGroup;

  // expose enum to template
  TaskStatus = TaskStatus;

  constructor(
    private taskService: TaskService,
    private userService: UserService,
    private projectService: ProjectService,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadData();
  }

  initForm(): void {
    this.taskForm = this.formBuilder.group({
      title: ['', [Validators.required]],
      description: [''],
      assignedToId: ['', Validators.required],
      reporterId: ['', Validators.required],
      projectId: ['', Validators.required],
      status: [TaskStatus.New],
    });
  }

  loadData(): void {
    let currentUserId = this.authService.getUserId();
    forkJoin({
      users: this.userService.getAll(),
      projects: this.projectService.getAll(),
      tasks: this.taskService.getAll(),
    }).subscribe({
      next: ({ users, projects, tasks }) => {
        this.tasks = tasks;
        this.users = users;
        this.projects = projects;
        this.applyFilter();
        this.cdr.detectChanges();
      },
      error: (err) => console.log(err),
    });
  }

  applyFilter(): void {
    if (this.filterStatus === '') {
      this.filteredTasks = [...this.tasks];
    } else {
      this.filteredTasks = this.tasks.filter(t => t.status === Number(this.filterStatus));
    }
  }

  openCreateModal(): void {
    // TODO: set isEditMode=false, selectedTaskId=null, reset form, set showModal=true
    this.isEditMode = false;
    this.selectedTaskId = null;
    this.taskForm.reset({ status: TaskStatus.New });
    this.showModal = true;
  }

  openEditModal(task: TaskResponse): void {
    // TODO: set isEditMode=true, selectedTaskId=task.id, patch form with task values, set showModal=true
    this.isEditMode = true;
    this.selectedTaskId = task.id;
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      assignedToId: task.assignedToId,
      reporterId: task.reporterId,
      projectId: task.projectId,
      status: task.status,
    });
    this.showModal = true;
  }

  closeModal(): void {
    // TODO: set showModal=false
    this.showModal = false;
  }

  onSubmit(): void {
    // TODO: if form invalid call markAllAsTouched and return
    // if isEditMode: call taskService.update, on next closeModal + loadData
    // else: call taskService.add, on next closeModal + loadData
    if (this.taskForm.valid) {
      const { title, description, assignedToId, reporterId, projectId, status } = this.taskForm.value;

      if (this.isEditMode && this.selectedTaskId) {
        const request: UpdateTaskRequest = {
          title,
          description,
          status:       Number(status),
          assignedToId: Number(assignedToId),
          reporterId:   Number(reporterId),
          projectId:    Number(projectId)
        };
        this.taskService.update(this.selectedTaskId, request).subscribe({
          next: () => { this.closeModal(); this.loadData(); },
          error: (err) => console.log(err)
        });
      } else {
        const request: CreateTaskRequest = {
          title,
          description,
          assignedToId: Number(assignedToId),
          reporterId:   Number(reporterId),
          projectId:    Number(projectId)
        };
        this.taskService.add(request).subscribe({
          next: () => { this.closeModal(); this.loadData(); },
          error: (err) => console.log(err)
        });
      }
    } else {
      this.taskForm.markAllAsTouched();
    }
  }

  deleteTask(id: number): void {
    // TODO: confirm with window.confirm, then call taskService.delete, on next call loadData
    if (!window.confirm('Are you sure you want to delete this task?')) return;
    this.taskService.delete(id).subscribe({
      next: () => {
        this.loadData();
      },
      error: (err) => console.log(err),
    });
  }

  // --- helpers (repetitive — already filled) ---

  getStatusLabel(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.New:
        return 'New';
      case TaskStatus.InProgress:
        return 'In Progress';
      case TaskStatus.Done:
        return 'Done';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.New:
        return 'badge badge-new';
      case TaskStatus.InProgress:
        return 'badge badge-progress';
      case TaskStatus.Done:
        return 'badge badge-done';
      default:
        return 'badge';
    }
  }
}
