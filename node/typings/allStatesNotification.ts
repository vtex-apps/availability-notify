export interface AllStatesNotification {
  recorder: Recorder
  domain: string
  orderId: string
  currentState: string
  lastState: string
  currentChangeDate: string
  lastChangeDate: string
}

export interface Recorder {
  _record: Record
}

export interface Record {
  'x-vtex-meta': XVtexMeta
  'x-vtex-meta-bucket': XVtexMeta
}

export interface XVtexMeta {
  [key: string]: unknown
}
