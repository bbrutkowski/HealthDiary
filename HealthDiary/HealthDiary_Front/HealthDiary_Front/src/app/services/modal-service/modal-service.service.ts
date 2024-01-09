import { DOCUMENT } from '@angular/common';
import { ComponentFactoryResolver, Inject, Injectable, Injector, TemplateRef } from '@angular/core';
import { Subject } from 'rxjs';
import { PopupModalComponent } from 'src/app/helpers/popup-modal/popup-modal/popup-modal.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalNotifier?: Subject<string>;

  public constructor(private resolver: ComponentFactoryResolver,
                     private injector: Injector,
                     @Inject(DOCUMENT) private document: Document) 
                    { }

  open(content: TemplateRef<any>, options?: {body?: string; title?: string}) {
    const modalComponentFactory = this.resolver.resolveComponentFactory(PopupModalComponent);
    const contentViewRef = content.createEmbeddedView(null);
    const modalComponent = modalComponentFactory.create(this.injector, [contentViewRef.rootNodes]);

    modalComponent.instance.modalBody = options?.body;
    modalComponent.instance.modalTitle = options?.title;

    modalComponent.instance.closeEvent.subscribe(() => {
      this.closeModal()
    });

    modalComponent.instance.submitEvent.subscribe(() => {
      this.submitModal()
    });

    modalComponent.hostView.detectChanges();

    this.document.body.appendChild(modalComponent.location.nativeElement);

    this.modalNotifier = new Subject();
    return this.modalNotifier?.asObservable();
  }

  private submitModal() {
    this.modalNotifier?.next('confirm')
    this.closeModal();
  }

  private closeModal() {
    this.modalNotifier?.complete(); 
  }
}
