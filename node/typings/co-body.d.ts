declare module 'co-body' {
  function json(req: any, opts?: any): Promise<any>
  export = json
  export { json }
}
