import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgFor, NgIf, DatePipe } from '@angular/common';

import { ProjectService } from '../../../core/services/project.service';
import { ProjectResponse, CreateProjectRequest } from '../../../core/models/project.model';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf, DatePipe],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.scss',
})
export class ProjectListComponent implements OnInit {
  projects: ProjectResponse[] = [];

  showModal = false;
  isEditMode = false;
  selectedProjectId: number | null = null;

  projectForm!: FormGroup;

  constructor(
    private projectService: ProjectService,
    private formBuilder: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadProjects();
  }

  initForm(): void {
    // TODO: build projectForm with fields: name (required), description
    this.projectForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: [''],
    });
  }

  loadProjects(): void {
    // TODO: call projectService.getAll(), assign to this.projects, call cdr.detectChanges()
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.cdr.detectChanges();
      },
      error: (err) => console.log(err),
    });
  }

  openCreateModal(): void {
    // TODO: set isEditMode=false, selectedProjectId=null, reset form, set showModal=true
    this.showModal = true;
    this.isEditMode = false;
    this.selectedProjectId = null;
    this.projectForm.reset();
  }

  openEditModal(project: ProjectResponse): void {
    // TODO: set isEditMode=true, selectedProjectId=project.id, patchValue, set showModal=true
    this.isEditMode = true;
    this.selectedProjectId = project.id;
    this.projectForm.patchValue({
      name: project.name,
      description: project.description,
    });
    this.showModal = true;
  }

  closeModal(): void {
    // TODO: set showModal=false
    this.showModal = false;
  }

  onSubmit(): void {
    // TODO: if invalid markAllAsTouched and return
    // if isEditMode: call projectService.update, on next closeModal + loadProjects
    // else: call projectService.add, on next closeModal + loadProjects
    if (this.projectForm.valid) {
      const { name, description } = this.projectForm.value;
      if (this.isEditMode && this.selectedProjectId) {
        this.projectService.update(this.selectedProjectId, { name, description }).subscribe({
          next: () => {
            this.showModal = false;
            this.loadProjects();
          },
        });
      } else {
        this.projectService.add({ name, description }).subscribe({
          next: () => {
            this.showModal = false;
            this.loadProjects();
          },
        });
      }
    } else {
      this.projectForm.markAllAsTouched();
    }
  }

  deleteProject(id: number): void {
    // TODO: window.confirm, then projectService.delete, on next loadProjects
    if (!window.confirm('Are you sure you want to delete this project?')) return;
    this.projectService.delete(id).subscribe({
      next: () => this.loadProjects(),
      error: (err) => console.log(err),
    });
  }
}
